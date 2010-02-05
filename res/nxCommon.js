function MadMax(value1, value2){
	return (value1>value2)?value1:value2;
}
function MadMin(value1, value2){
	return (value1<value2)?value1:value2;
}
function toDouble(texto){
	var temp = '';
	texto = texto + '';
	for (i=0; i<texto.length; i++)
		temp += (texto.charAt(i)!='.') ? (texto.charAt(i)==',') ? '.':texto.charAt(i):'';
	if(temp == "0.00"){
		temp=0;
	}
	return parseFloat(temp);
}
function Exists(id)
{
    return ($('#' + id).length>0);
}

function DoScroll(id)
{
    if (Exists(id + '_Header')) //if header is Visible
    {
        $('#' + id).css('left', $('#' + id + '_Content').scrollLeft()*-1);
	}
	try
	{		
		$('#' + id + '_txtPosX').val($('#' + id + '_Content').scrollLeft());
		$('#' + id + '_txtPosY').val($('#' + id + '_Content').scrollTop());
	}catch(ex)
	{
		alert(ex);
	}
}
function PutScroll(id)
{
	try
	{
		var auxX = $('#' + id + '_txtPosX').val();
		var auxY = $('#' + id + '_txtPosY').val();
		$('#' + id + '_Content').scrollLeft(auxX);
		$('#' + id + '_Content').scrollTop(auxY);
	
		DoScroll(id);
	}catch(ex)
	{
		alert(ex);
	}
}

function sleep(milliseconds) {
  var start = new Date().getTime();
  for (var i = 0; i < 1e7; i++) {
    if ((new Date().getTime() - start) > milliseconds){
      break;
    }
  }
}
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
jQuery.fn.visible = function(visibleIt) {
    //sets object(s) to visibility or returns the visibility state of the first element
    if (visibleIt == null || visibleIt == undefined) //nothing passed, just return state of first element
    {
        return $(this).css("visibility");
    }
    else //setting element(s)
    {
        $(this).each(function(){
            $(this).css("visibility", (visibleIt) ? "visible" : "hidden");
        });
        return this; //keep chain
    }
};
jQuery.fn.display = function(displayMode) {
    //sets object(s) display mode or returns the display mode of the first element
    if (displayMode == null || displayMode == undefined) //nothing passed, just return state of first element
    {
        return $(this).css("display");
    }
    else //setting element(s)
    {
        $(this).each(function(){
            $(this).css("display", displayMode);
        });
        return this; //keep chain
    }
};
jQuery.fn.readonly = function(readonlyIt) {
    //sets object(s) to readonly or returns the readonly state of the first element
    if (readonlyIt == null || readonlyIt == undefined) //nothing passed, just return state of first element
    {
        return !($(this).attr("readonly") == "readonly");
    }
    else //setting element(s)
    {
        $(this).each(function(){
            if (readonlyIt)
            {
                $(this).removeAttr("readonly");
            }
            else
            {
                $(this).attr("readonly", "readonly");
            }
        });
        return this; //keep chain
    }
};
jQuery.fn.enabled = function(enableIt) {
    //sets object(s) to enabled/disabled or returns the enabled state of the first element
    if (enableIt == null || enableIt == undefined) //nothing passed, just return state of first element
    {
        return !($(this).attr("disabled") == "disabled");
    }
    else //setting element(s)
    {
        $(this).each(function(){
            if (enableIt)
            {
                $(this).removeAttr("disabled");
            }
            else
            {
                $(this).attr("disabled", "disabled");
            }
        });
        return this; //keep chain
    }
};
jQuery.fn.centerScreen = function(loaded) {
    var obj = this;
    var top = document.documentElement.scrollTop || window.pageYOffset || 0;
    var left = document.documentElement.scrollLeft || window.pageXOffset || 0;
    
    if(!loaded) 
    {
        obj.css('top', top + $(window).height()/2-this.height()/2);
        obj.css('left', left + $(window).width()/2-this.width()/2);
        
        $(window).resize(function(){ obj.centerScreen(!loaded); });
    } 
    else 
    {
        obj.stop();
        obj.animate({ 
                top: top + $(window).height()/2-this.height()/2, 
                left: left + $(window).width()/2-this.width()/2
                }, 200, 'linear');
     }
     return this; //keep chain
}; 