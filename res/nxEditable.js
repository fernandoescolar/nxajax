var __editable = new EditableObjects();

function EditableObjects() {
	this.ParseToEditMode = ParseToEditMode;
	this.ExitFromEditMode = ExitFromEditMode;
	this.AddOnBlurExit = AddOnBlurExit;
	this.AddOnReturnExit = AddOnReturnExit;
	
	
	function ParseToEditMode(id)
	{
	    $('#' + id + '_view').hide();
	    $('#' + id).show();
	    var obj = $('#' + id);
	    obj.focus();
	    
	    if(obj[0].type == 'text')
	        if (obj.val() != '')
	            obj.select();
	}
	function ExitFromEditMode(id)
	{
	    var obj = $('#' + id);
	    if(obj[0].type == 'text' || obj[0].type == 'textarea')
	    {
	        var value = obj.val();
	        if (value == '')
	            value = '<i><font color=\"green\">Edit</font></i>';
	        if (obj[0].type == 'textarea')
	        {
	            value = value.replace(/\n/gi, "<br/>");
	        }
	    }
	    else if(obj[0].type == 'select-one')
	    {
	        value = obj[0].options[obj[0].selectedIndex].text;
	    }
	    $('#' + id + '_view').html(value);
	    obj.hide();
	    $('#' + id + '_view').show();
	    

	}
	function AddOnBlurExit(id, exitEventHandler)
	{
	    var obj = $('#' + id)[0];
	    var oldFunction = (obj.onblur) ? obj.onblur : function() {};
	    obj.onblur = function() { oldFunction(); eval(exitEventHandler); ExitFromEditMode(id);}
	}
	function AddOnReturnExit(id)
	{
	    var obj = $('#' + id)[0];
	    var oldFunction = (obj.onkeydown) ? obj.onkeydown : function() {};
	    obj.onkeydown = function(event) { 
	        oldFunction(event); 
	        var tecla =(document.all)?window.event.keyCode:event.which;
	        if(tecla==13)
	            obj.blur();
	    }
	}
}