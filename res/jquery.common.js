function DoScroll(id)
{
    if ($('#' + id + '_Header').exists()) //if header is Visible
    {
        $('#' + id + '_Header').css('left', $('#' + id + '_Content').scrollLeft()*-1);
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

jQuery.fn.exists = function() {
    return ($(this).length>0);
};

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
        return ($(this).attr("readonly") == "readonly");
    }
    else //setting element(s)
    {
        $(this).each(function(){
            if (!readonlyIt)
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
jQuery.extend({
    nxParam: function(a) {
        var s = [];
        function add(key, value) {
            // If value is a function, invoke it and return its value
            value = jQuery.isFunction(value) ? value() : value;
            s[s.length] = encodeURIComponent(key) + "=" + encodeURIComponent(value);
        }
        
        // Serialize the form elements
        jQuery.each(a, function() {
         add(this.id, this.value);
        });
        // Return the resulting serialization
        return s.join("&").replace(/%20/g, "+");
    }
});
jQuery.fn.nxSerialize = function() {
    return $.nxParam(this.nxSerializeArray());
};
jQuery.fn.nxSerializeArray = function() {
    var rselectTextarea = /select|textarea/i;
    var rinput = /color|date|datetime|email|hidden|month|number|password|range|search|tel|text|time|url|week/i;
    
    return this.map(function() {
        return this.elements ? jQuery.makeArray(this.elements) : this;
    })
        .filter(function() {
            return this.id && !this.disabled &&
                (this.checked || rselectTextarea.test(this.nodeName) ||
                    rinput.test(this.type));
        })
        .map(function(i, elem) {
            var val = jQuery(this).val();

            return val == null ?
                null :
                    jQuery.isArray(val) ?
                    jQuery.map(val, function(val, i) {
                        return { id: elem.id, value: val };
                    }) :
                    { id: elem.id, value: val };
        }).get();
};
jQuery.fn.nxFixedSerialize = function() {
    var serialized = this.nxSerialize();
    $(this).find(':checkbox').each(function() {
        var tofind = $(this).attr('id') + "=" + $(this).val();
        var toreplace = $(this).attr('id') + "=" + (this.checked ? 'checked' : 'unchecked');
        
        if (this.checked) { serialized = serialized.replace(tofind, toreplace); }
        else { serialized += ((serialized != '') ? "&" : "") + toreplace; }
    });

    $(this).find(':radio').each(function() {
        var tofind = $(this).attr('id') + "=" + $(this).val();
        var toreplace = $(this).attr('id') + "=" + (this.checked ? 'checked' : 'unchecked');

        if (this.checked) { serialized = serialized.replace(tofind, toreplace); }
        else { serialized += ((serialized != '') ? "&" : "") + toreplace; }
    });
    return serialized;
};

