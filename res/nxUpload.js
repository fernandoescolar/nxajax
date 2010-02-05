var __uploader = new Uploader();

function Uploader() {

    this.GetFileContent = GetFileContent;
    this.AjaxUpload = AjaxUpload;
    this.RequestDone = RequestDone;
    
    function GetFileContent(id) 
    {
	    var filename = $(id).value;
	    

	    // request local file read permission
	    try {
		    netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
	    } catch (e) {
		    alert("Permission to read file was denied.");
	    }
    	
	    // open the local file
	    var file = Components.classes["@mozilla.org/file/local;1"]
		    .createInstance(Components.interfaces.nsILocalFile);
	    file.initWithPath( filename );		
	    var stream = Components.classes["@mozilla.org/network/file-input-stream;1"]
		    .createInstance(Components.interfaces.nsIFileInputStream);
	    stream.init(file,	0x01, 00004, null);
	    var bstream =  Components.classes["@mozilla.org/network/buffered-input-stream;1"]
		    .getService();
	    bstream.QueryInterface(Components.interfaces.nsIBufferedInputStream);
	    bstream.init(stream, 1000);
	    bstream.QueryInterface(Components.interfaces.nsIInputStream);
	    var binary = Components.classes["@mozilla.org/binaryinputstream;1"]
		    .createInstance(Components.interfaces.nsIBinaryInputStream);
	    binary.setInputStream (stream);

	    return escape(binary.readBytes(binary.available()));
    }

    function AjaxUpload(id) 
    {
	    var content = __uploader.GetFileContent(id);
	    var filename = $(id).value;
	    
	    $(id + '_Button').disabled = true;
	    
	    
	    if (!document.all)
	    {
	        // request more permissions
	        try {
		        netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
	        } catch (e) {
		        alert("Permission to read file was denied.");
	        }
	    }

	    var http_request = false;
	    if (window.XMLHttpRequest)
		{
	        http_request = new XMLHttpRequest();
	    }
	    else if (window.ActiveXObject)
		{
			try {
			http_request = new ActiveXObject('Msxml2.XMLHTTP') ;
			} catch(e){
			http_request = new ActiveXObject('Microsoft.XMLHTTP');}
		}
		
	    if (!http_request) {
		    alert('Cannot create XMLHTTP instance');
		    return false;
	    }

	    // prepare the MIME POST data
	    var boundaryString = 'capitano';
	    var boundary = '--' + boundaryString;
	    var requestbody = boundary + '\n' 
	    + 'Content-Disposition: form-data; name="' + id + '"; filename="' 
		    + filename + '"' + '\n' 
	    + 'Content-Type: application/octet-stream' + '\n' 
	    + '\n'
	    + content
	    + '\n'
	    + boundary;

	    //document.getElementById('sizespan').innerHTML = 
		//    "requestbody.length=" + requestbody.length;
    	
	    // do the AJAX request
	    http_request.onreadystatechange = requestdone;
	    http_request.open('POST', url, true);
	    http_request.setRequestHeader("Content-type", "multipart/form-data; \
		    boundary=\"" + boundaryString + "\"");
	    http_request.setRequestHeader("Connection", "close");
	    http_request.setRequestHeader("Content-length", requestbody.length);
	    http_request.send(__uploader.RequestDone);

    }

    function RequestDone() {
	    if (this.readyState == 4) {
		    if (this.status == 200) {
			    result = this.responseText;
			    //document.getElementById('myspan').innerHTML = result;            
		    } else {
			    alert('There was a problem with the request.');
		    }
		    //document.getElementById('ajaxbutton').disabled = false;
	    }
    }


}