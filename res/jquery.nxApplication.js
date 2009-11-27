
jQuery.extend({
    nxApplication : {
	    loadId: 'loading',
        loader: function(isOn) {
		    if(this.loadId && $('#' + this.loadId).exists())
		    {
                try
                {
			        if(isOn)
			            $('#' + this.loadId).fadeIn('fast', function() {$(this).fadeTo('slow', 0.80); });
			        else
			            $('#' + this.loadId).fadeOut('slow');
			    }catch(ex) { 
			        //There is not a Loader DIV
			    }
		    }
	    },
	    getSpecialParamWithName : function(name) {
	        var now = new Date();
		    var times = now.getHours();
		    times  += '.' + now.getMinutes();
		    times  += '.' + now.getSeconds();
		    return '&' + name + '=' + times;
	    },
        getSpecialParam : function() {
		    return this.getSpecialParamWithName('__VIEWSTATE');
	    },
		purgeValue: function (value) {
	        try
		    {
		        value = value.replace(/\n/g, "\\\\n").replace(/\r/g, "\\\\r");
		        value = value.replace(/\'/g, "\\\\\'");
		    }catch(ex) {
		        //not is a string...
		    }
		    return value;
	    },
	    DoPostBackWithValue: function (id, action, page, value)
	    {
		    var val = this.purgeValue(value);
		    var param = '__id=' + id + '&__action=' + action + '&__value=' + val + this.getSpecialParam();
		    $.ajaxQueue.get(null, {
                url: page + '?' + param,
                beforeSend: function(objeto){
                    //loadingImg FadeIn
                    $.nxApplication.loader(true);
                },
                complete: function(objeto, exito){
                    //LoadingImg FadeOut
                    $.nxApplication.loader(false);
                },
                success: function(html){ $.nxApplication.OnDonePostBackEventHandler(html); }
            });
	    },
	    DoAsyncPostBackWithValue: function(id, action, page, value, loadingImg)
	    {
		    var val = this.purgeValue(value);
		    var param = '__id=' + id + '&__action=' + action + '&__value=' + val + this.getSpecialParam();
    		
		    $.ajax({
		        url: page + '?' + param,
		        beforeSend: function(objeto){
                    //loadingImg FadeIn
                    if ($('#' + loadingImg).exists())
                        $('#' + loadingImg).fadeIn('slow');
                },
                complete: function(objeto, exito){
                    if ($('#' + loadingImg).exists())
                        $('#' + loadingImg).fadeOut('slow');
                },
                success: function(html){
                    $.nxApplication.OnDonePostBackEventHandler(html);
                }
		    });
	    },
	    DoPostBack: function(id, action, page, isAsync, loadingImg)
	    {	
		    var obj = $('#' + id)[0];
		    var tipo = obj.type;
		    var value = "";

		    if (tipo == 'select-multiple')
		    {
			    var n = 0;
			    for (j = 0; j < obj.options.length; j ++ )
				    if (obj.options[j].selected)
					    value = obj.options[j].value;
		    }
		    else if (tipo == 'radio' || tipo == 'checkbox')
			    value = obj.checked;
		    else if (tipo == 'submit')
		    {
		        var temp = $('#' + id).closest("form").attr('id'); // obj.form.id;
			    this.SubmitForm(temp, id);
			    return;
		    }
		    else 
			    value = obj.value;
		    if (isAsync)
		        this.DoAsyncPostBackWithValue(id, action, page, value, loadingImg);
		    else
		        this.DoPostBackWithValue(id, action, page, value);
	    },
        LoadPane: function(id, action, param)
        {
            var hash = id + '+' + action + '+' + param;
            $.historyLoad(hash);
        },
	    internalLoadPane: function(id, action, param)
	    {
		    if (param)
		        param += '&__parent=' + id;
		    else
			    param = '__parent=' + id;
    		
		    param += this.getSpecialParamWithName('time');
		    //$('#' + id).html("");
    		
		    $.ajaxQueue.get(null, {
                    url: action + '?' + param,
                    beforeSend: function(objeto){
                        //loadingImg FadeIn
                        $.nxApplication.loader(true);
                    },
                    complete: function(objeto, exito){
                        //LoadingImg FadeOut
                        $.nxApplication.loader(false);
                    },
                    success: function(html){ $('#' + id).html(html); $.nxApplication.OnDoneLoadPane(id); }
            });
	    },
        pageload: function(hash) {
		    // hash doesn't contain the first # character.
		    if(hash) {
		        var s = hash.split('+');
		        var id = s[0];
		        var action = s[1];
		        var param = (s.length>2) ? s[2] : null;
			    // restore ajax loaded state
			    if (this.internalLoadPane)
			        this.internalLoadPane(id, action, param);
			    else
			        $.nxApplication.internalLoadPane(id, action, param);
		    } 
	    },
	    SubmitForm: function(f, root)
	    {
		    if (!f)
		    {
			    alert('there is not Form');
			    return;
		    }
		    var formData = root + '=';
		    if ($('#' + root).val())
		        formData += $('#' + root).val();
		    else if ($('#' + root).html())
		        formData += $('#' + root).html();
		    else
		        formData += root;	        
		    formData += '&' + $('#' + f).nxSerialize();
		    formData = '__id=' + root + '&' + formData + this.getSpecialParam();
		    
		    $.ajaxQueue.post(null, {
                    url: $('#' + f).attr('action'),
                    type: 'POST',
                    data: formData,
                    beforeSend: function(objeto){
                        //loadingImg FadeIn
                        $.nxApplication.loader(true);
                    },
                    complete: function(objeto, exito){
                        //LoadingImg FadeOut
                        $.nxApplication.loader(false);
                    },
                    success: function(html){ $.nxApplication.OnDonePostBackEventHandler(html); }
            });
	    },
	    OnDoneLoadPane: function(id)
	    {
		    var times = 0;	
		    setTimeout("$.nxApplication.TryLoadPane('" + id + "', " + times + ")", 100);
	    },
	    TryLoadPane: function(id, times)
	    {
		    try {
				    eval($('#' + id + '_CONTAINED_postscript').val());
				    return true;
		    }
		    catch(e)
		    {
			    //alert('Si vd. navega con internet explorer puede ser que se le muestre este mensaje...\n\rNo se asuste y presione aceptar.');
			    //alert('Loading Buffers...');
			    //if (times>100)
			    if (times>3)
			    {
				    //alert('Ha ocurrido un error: Se ha superado el tiempo de espera...\n\r Intentelo de nuevo mas tarde.');
				    if ($('#' + id + '_CONTAINED_postscript').exists())
				        alert(e.message + "\n" + $('#' + id + '_CONTAINED_postscript').val()  + "\nTimes: " + times );
				    else
				        alert(e.message + "\n" + id + '_CONTAINED_postscript'  + "\nTimes: " + times );
				    return;
			    }
			    //alert(e.message);
		    }
		    times++;
		    setTimeout("$.nxApplication.TryLoadPane('" + id + "', " + times + ")", 300);
	    },
	    OnDonePostBackEventHandler: function(data)
	    {
		    try
		    {
			    eval(data);
		    }
		    catch(ex)
		    {
			    alert(ex.message + '\n' + data);
		    }
	    },
	    Init: function (loaderId)
	    {
	        this.loadId = loaderId;
            this.loader(false);
	    }
    }
});