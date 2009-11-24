function getRealLeftById(id)
{
    return getRealLeft($('#' + id)[0]);
}
function getRealTopById(id)
{
    return getRealTop($('#' + id)[0]);;
}
function getRealLeft(obj)
{
    var xPos = 0;
    tempEl = obj;
    do
    {
        xPos += tempEl.offsetLeft;
        tempEl = tempEl.offsetParent;
    }while (tempEl != null);
    return xPos;
}
function getRealTop(obj)
{
    tempEl = obj;
    var yPos = 0;
    do
    {
        yPos += tempEl.offsetTop;
        tempEl = tempEl.offsetParent;
    }while (tempEl != null);
    return yPos;
}

var __dragndrop = new DragnDrop();

function DragnDrop()
{
    this.Load = Load;
    this.Catch = Catch;
    this.Release = Release;
    this.Move = Move;
    this.Init = Init;
    
    var startx, starty, objx, objy;
    var currentDst = null, originalParent = null;
    var manager;
    var containerElements;
    var configuration;
    var className, selectedClassName
    
    function Init(managerId, cName, selectedCName)
    {
        className = cName;
        selectedClassName = selectedCName;
        manager = managerId;
        configuration = $('#' + manager).val().split("|");
        var items = configuration[1].split(';');
        var sizes = configuration[0].split(';');
        var disposition = configuration[2].split(';');
        
        containerElements = new Array(items.length);
        
        for(var i=0;i<items.length; i++)
        {
            var size = sizes[i].split(',');
            var item = $('#' + items[i])[0];
            item.style.width = size[0] + 'px';
            item.style.height = size[1] + 'px';
            
            containerElements[i] = new Element(items[i]);
            containerElements[i].id = items[i];
            containerElements[i].height = size[1];
            containerElements[i].width = size[0];
        }
        for(i=0;i<disposition.length;i++)
        {
            if (disposition[i] != '')
            {
                var temp = disposition[i].split(',');
                for(var j=0; j<temp.length; j++)
                {
                    if(temp != '')
                    {
                        var container = $('#' + items[i])[0];
                        var div = $('#' + temp[j])[0];
                        var w = purge(container.style.width);
                        var h = purge(container.style.height);
                        if (w)
                        {
                            if (w < purge(div.style.width))
                                container.style.width = (purge(div.style.width)) + 'px';
                        }
                        else
                            container.style.width = (purge(div.style.width)) + 'px';
                            
                        if (h)
                        {
                            if (h < purge(div.style.height))
                                container.style.height = (purge(div.style.height)) + 'px';
                        }
                        else
                            container.style.height = (purge(div.style.height)) + 'px';
                            
                        container.appendChild(div);
                        containerElements[i].Append(temp[j]);
                    }
                }
            }
        }
    }
    
    function Load(id)
    {
        var obj = $(id + '_Title');
        obj.onmouseover=function() { this.style.cursor = "move"; }
        obj.zIndex = -10;
        obj.onmousedown = function(event)
        {
            __dragndrop.Catch(id, event);
            return false;
        };
    }
    function Catch(id, event)
    {
        objx = getRealLeftById(id);
        objy = getRealTopById(id);
        
        var obj = $(id);
        obj._style = (obj.style) ? obj.style : obj;
        obj._style.top = objy + 'px';
        obj._style.left = objx + 'px';
        
        obj._style.position = 'absolute';
        obj._style.zIndex = 100;
        
        if (window.event)
        {
            startx=window.event.clientX+document.documentElement.scrollLeft+document.body.scrollLeft;
	        starty=window.event.clientY+document.documentElement.scrollTop+document.body.scrollTop;
        }
        else
        {
            startx=event.clientX+window.scrollX;
	        starty=event.clientY+window.scrollY;
        }
        
        MarkParent(obj, startx, starty);
        originalParent = currentDst;
        
        var title = $(id + '_Title');
        title.onmousemove = function(event)
        {
            __dragndrop.Move(id, event);
        };
        title.onmouseup = function(event)
        {
            __dragndrop.Release(id, event);
        };
    }
    function Release(id, event)
    {
        var obj = $('#' + id)[0];
        
        startx = 0;
        starty = 0;
        
        objx = 0;
        objy = 0;
        
        $('#' + id + '_Title')[0].onmousemove=null;
        $('#' + id + '_Title')[0].onmouseup=null;
            
        obj.style.position = '';
	    obj.style.top = '0px';
	    obj.style.left = '0px';
        obj.style.zIndex = 1;
        
	    var dstObj = $('#' + (currentDst != null)? containerElements[currentDst].id : containerElements[originalParent].id)[0];
	    if (currentDst != null)
	    {
	        var top = 0;
	        if (containerElements[currentDst].disposition.length>0)
	            for(var i=0; i<containerElements[currentDst].disposition.length; i++)
	                top += purge($(containerElements[currentDst].disposition[i]).style.height);
	                
	         $(id).style.top = top + 'px';
	    }
	    
	    if (Exits('_newdiv_'))
            dstObj.removeChild($('#_newdiv_')[0]);
	    dstObj.appendChild(obj);   
	
	    if (currentDst != null)
	    {
	        if (originalParent != null)
	            containerElements[originalParent].Remove(id);
	            
	        containerElements[currentDst].Append(id);
	    }
	    RefreshDisposition();
    	currentDst = null;
    	originalParent = null;
    }
    function Move(id, event)
    {  
        var currentx, currenty;
        if(window.event)
        {    
            currentx=window.event.clientX+document.documentElement.scrollLeft+document.body.scrollLeft;
            currenty=window.event.clientY+document.documentElement.scrollTop+document.body.scrollTop;
        }  
        else
        {
            currentx=event.clientX+window.scrollX;
            currenty=event.clientY+window.scrollY;
        }
    	MarkParent($('#' + id)[0], currentx, currenty);
        $('#' + id)[0].style.left=(objx+currentx-startx)+"px";
        $('#' + id)[0].style.top=(objy+currenty-starty)+"px";

        IgnoreEvents(event);
    }
    function IgnoreEvents(event)
    {
        if(window.event)
        {
            window.event.cancelBubble=true;
            window.event.returnValue=false;
        }
        else
        {
            event.preventDefault();
        }
    }
   
    function MarkParent(obj, x, y)
    {
        if (currentDst != null)
    	    containerElements[currentDst].UnMark(className, obj);
    	
    	currentDst = FindParent(x, y);
    	
    	if (currentDst != null)
    	    containerElements[currentDst].Mark(selectedClassName, obj, x, y);
    }
    function FindParent(x, y)
    {
        var elements = getContainersInPosition(x, y);
        if (elements.length == 1)
            return elements[0];
        else if (elements.length > 0)
            return elements[0];
        else
            return null;   
    }
    function getContainersInPosition(x, y) 
    {
        var classElements = new Array();
        
        for (i = 0, j = 0; i < containerElements.length; i++) 
        {
            var top, left, width, height;
            var obj = $(containerElements[i].id);
            left = getRealLeft(obj);
            top = getRealTop(obj);
            width = obj.offsetWidth;
            height = obj.offsetHeight;
           
            if (x >= left && x <= (left + width) && y >= top && y <= (top + height))
            {
                classElements[j] = i;
                j++;
            }
        }
        return classElements;
    }
    
    function RefreshDisposition()
    {
        var disposition = '';
        for(var i=0; i<containerElements.length;i++)
        {
            if (i>0)
                disposition += ';';
                
            for(var j=0; j<containerElements[i].disposition.length; j++)
            {
                if (j>0)
                    disposition += ',';
                    
                disposition += containerElements[i].disposition[j];
            }
        }
        $(manager).value = configuration[0] + '|' + configuration[1] + '|' + disposition;
    }
    function purge(value)
    {
        value = value.replace('px', '');
        value = value.replace('pt', '');
        return parseInt(value);
    }
    function Element(id)
    {
        this.id = (id)? id : '';
        this.height = 0;
        this.width = 0;
        this.disposition = new Array();
        this.Mark = Mark;
        this.UnMark = UnMark;
        this.Remove = Remove;
        this.Append = Append;
        
        function Mark(className, obj, x, y)
        {
            var me = $('#' + this.id)[0];
            var myW = me.offsetWidth;
            var myH = me.offsetHeight;
            var objW = obj.offsetWidth;
            var objH = obj.offsetHeight;
            var totalX = 0;
            var totalY = 0;
            for(var i=0; i<this.disposition.length; i++)
            {
                var item = $(this.disposition[i]);
                if(item.id!=obj.id)
                {
                    totalX += item.offsetWidth;
                    totalY += item.offsetHeight;
                }
            }
            
            //objW += totalX;
            objH += totalY;
            
            if (myW)
            {
                if (myW < objW)
                    me.style.width = objW + 'px';
            }
            else
                me.style.width = objW + 'px';
                
            if (myH)
            {
                if (myH < objH)
                    me.style.height = objH + 'px';
            }
            else
                me.style.height = objH + 'px';
                    
    	    //me.className  = className;
    	    var newdiv = document.createElement('div');
            newdiv.setAttribute('id', '_newdiv_');
            newdiv.className = className;
            newdiv.style.width = obj.offsetWidth + 'px';
            newdiv.style.height = obj.offsetHeight + 'px';
            me.appendChild(newdiv);
        }
        function UnMark(className, obj)
        {
            var me = $(this.id);
            var totalX = 0;
            var totalY = 0;
            for(var i=0; i<this.disposition.length; i++)
            {
                var item = $(this.disposition[i]);
                if(item.id!=obj.id)
                {
                    totalX += item.offsetWidth;
                    totalY += item.offsetHeight;
                }
            }
            me.style.width = ((this.width<totalX)? totalX : this.width) + 'px';
            me.style.height = ((this.height<totalY)? totalY : this.height) + 'px';
            var olddiv = $('#_newdiv_')[0];
            me.removeChild(olddiv);
        }
        function Remove(id)
        {
            var dis = new Array();
            for(var i=0, j=0; i<this.disposition.length; i++)
            {
                if(id.toLowerCase() != this.disposition[i].toLowerCase())
                {
                    dis[j] = this.disposition[i];
                    j++;
                }
            }
            this.disposition = dis;
        }
        function Append(id)
        {
            this.disposition[this.disposition.length] = id;
        }
    }
};