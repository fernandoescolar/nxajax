function toDouble(texto){
	var temp = '';
	texto = texto + '';
	for (i=0; i<texto.length; i++)
		temp += (texto.charAt(i)!='.') ? (texto.charAt(i)==',') ? '.':texto.charAt(i):'';
	if(temp == "0.00"){
		temp=0;
	}
	var result = parseFloat(temp);
	if (result == NaN)
	    result = 0;
	return result;
}

var __textbox = new TextBox();


function TextBox() {

	this.LoadMaxLength = LoadMaxLength;
	this.LoadUnsignedFormat = LoadUnsignedFormat;
	this.LoadUnsignedFormatAndMaxLength = LoadUnsignedFormatAndMaxLength;
	this.LoadSignedFormat = LoadSignedFormat;
	this.LoadSignedFormatAndMaxLength = LoadSignedFormatAndMaxLength;
	this.LoadUnsignedFormatWithOnBlur = LoadUnsignedFormatWithOnBlur;
	this.LoadUnsignedFormatAndMaxLengthWithOnBlur = LoadUnsignedFormatAndMaxLengthWithOnBlur;
	this.LoadSignedFormatWithOnBlur = LoadSignedFormatWithOnBlur;
	this.LoadSignedFormatAndMaxLengthWithOnBlur = LoadSignedFormatAndMaxLengthWithOnBlur;
	this.NumberFormat = NumberFormat;
	this.putNumberFormat = putNumberFormat;
	this.hasPoint = hasPoint;
	this.OnlyNumbersWithPoint = OnlyNumbersWithPoint;
	this.OnlySignedNumbersWithPoint = OnlySignedNumbersWithPoint;
	this.OnlyNumbers = OnlyNumbers;
	this.OnlySignedNumbers = OnlySignedNumbers;
	this.CheckLength = CheckLength;
	
	
	function LoadMaxLength(id, max){
		var obj = $('#' + id)[0];
		if (obj == null)
		{
			alert(id);
			return ;
		}
		obj.onkeypress = function(evt){
				return __textbox.CheckLength(evt, this, max);
			}
		
	}
	function LoadUnsignedFormat(id, tipo){
	    LoadUnsignedFormatWithOnBlur(id, tipo, null);
	}
	function LoadUnsignedFormatWithOnBlur(id, tipo, onblur){
		var obj = $('#' + id)[0];
		if (obj == null)
		{
			alert(id);
			return ;
		}
		
		if (tipo==0){
			obj.onkeypress = function(evt){
				return __textbox.OnlyNumbers(evt);
			}		
			//obj.onblur = function (){
				//calcular();
			//}
		}
		else if (tipo>0){
			NumberFormat(obj, tipo);
			obj.onblur = function (){
				__textbox.NumberFormat(this, tipo);
				if(onblur)
				    eval(onblur);
			}
			obj.onkeypress = function(evt){
				return __textbox.OnlyNumbersWithPoint(evt, this);
			}
		}
		else{
		
		}
		obj.thestyle=(obj.style)? obj.style : obj;
		obj.thestyle.textAlign = 'right';
	}
	function LoadUnsignedFormatAndMaxLength(id, tipo, max)
	{
	    LoadUnsignedFormatAndMaxLengthWithOnBlur(id, tipo, max, null);
	}
	function LoadUnsignedFormatAndMaxLengthWithOnBlur(id, tipo, max, onblur){
		var obj = $('#' + id)[0];
		if (obj == null)
		{
			alert(id);
			return ;
		}
		
		if (tipo==0){
			obj.onkeypress = function(evt){
				return __textbox.OnlyNumbers(evt) && __textbox.CheckLength(evt, this, max);
			}		
			//obj.onblur = function (){
				//calcular();
			//}
		}
		else if (tipo>0){
			NumberFormat(obj, tipo);
			obj.onblur = function (){
				__textbox.NumberFormat(this, tipo);
				if(onblur)
				    eval(onblur);
			}
			obj.onkeypress = function(evt){
				return __textbox.OnlyNumbersWithPoint(evt, this) && __textbox.CheckLength(evt, this, max);
			}
		}
		else{
		
		}
		obj.thestyle=(obj.style)? obj.style : obj;
		obj.thestyle.textAlign = 'right';
	}
	function LoadSignedFormat(id, tipo){
	    LoadSignedFormatWithOnBlur(id, tipo, null);
	}
	function LoadSignedFormatWithOnBlur(id, tipo, onblur){
		var obj = $('#' + id)[0];
		if (obj == null)
		{
			alert(id);
			return ;
		}
		
		if (tipo==0){
			obj.onkeypress = function(evt){
				return __textbox.OnlySignedNumbers(evt, this);
			}		
			obj.onblur = function (){
					//calcular();
			}
		}
		else if (tipo>0){
			NumberFormat(obj, tipo);
			obj.onblur = function (){
					__textbox.NumberFormat(this, tipo);
					if(onblur)
				        eval(onblur);
			}
			obj.onkeypress = function(evt){
				return __textbox.OnlySignedNumbersWithPoint(evt, this);
			}
		}
		else{
		
		}
		obj.thestyle=(obj.style)? obj.style : obj;
		obj.thestyle.textAlign = 'right';
	}
	function LoadSignedFormatAndMaxLength(id, tipo, max){
	    LoadSignedFormatAndMaxLengthWithOnBlur(id, tipo, max, null);
	}
	function LoadSignedFormatAndMaxLengthWithOnBlur(id, tipo, max, onblur){
		var obj = $('#' + id)[0];
		if (obj == null)
		{
			alert(id);
			return ;
		}
		
		if (tipo==0){
			obj.onkeypress = function(evt){
				return __textbox.OnlySignedNumbers(evt, this) && __textbox.CheckLength(evt, this, max);
			}		
			//obj.onblur = function (){
					//calcular();
			//}
		}
		else if (tipo>0){
			NumberFormat(obj, tipo);
			obj.onblur = function (){
					__textbox.NumberFormat(this, tipo);
					if(onblur)
				        eval(onblur);
			}
			obj.onkeypress = function(evt){
				return __textbox.OnlySignedNumbersWithPoint(evt, this) && __textbox.CheckLength(evt, this, max);
			}
		}
		else{
		
		}
		obj.thestyle=(obj.style)? obj.style : obj;
		obj.thestyle.textAlign = 'right';
	}
	function NumberFormat(objeto, decimales){
		var resultado = '';
		var temp='';
		var hasPoint = false;
		var posComa;
		objeto.value = toDouble(objeto.value);
		__textbox.putNumberFormat(objeto, decimales);
	}
	function putNumberFormat(objeto, decimales){
		var resultado = '';
		var temp='';
		var hasPoint = false;
		var posComa;
		
		temp = objeto.value + '';
		for (i=0; i<temp.length; i++)
			if (temp.charAt(i)=='.'){
				hasPoint = true;		
				posComa = i;
			}
		posComa = hasPoint ? posComa:temp.length;
		for (i=posComa-1, j=1; i>=0; i--,j++){
			resultado = temp.charAt(i) + resultado;
			if (j%3==0&&i>0)
				resultado = '.' + resultado;
		}
		resultado += ',';
		if (hasPoint){
			for (i=0; i<decimales; i++)
				if(posComa+i<temp.length-1)
					resultado += temp.charAt(posComa + i + 1);
				else
					resultado += '0' + '';
		}
		else
			resultado += '00';
		objeto.value=resultado;
	}
	function hasPoint(texto){
		texto=texto + '';
		for (i=0; i<texto.length; i++)
			if (texto.charAt(i)==',')
				return true;		
		return false;
	}
	function OnlyNumbersWithPoint(evt, objeto){
		// NOTA: borrar = 8, Intro = 13, '0' = 48, '9' = 57	
		var nav4 = window.Event ? true : false;
		var key = nav4 ? evt.which : event.keyCode;
		if (key==46){
			nav4 ? key=14 : key=event.keyCode=44;
		}
		if (key==44 && __textbox.hasPoint(objeto.value)) key=14;
		return (key <= 13 || key == 44 || (key >= 48 && key <= 57));
	}
	function OnlySignedNumbersWithPoint(evt, objeto){
		// NOTA: borrar = 8, Intro = 13, '0' = 48, '9' = 57	
		var nav4 = window.Event ? true : false;
		var key = nav4 ? evt.which : event.keyCode;
		if (key==46){
			nav4 ? key=14 : key=event.keyCode=44;
		}
		if (key == 45){
			var temp = objeto.value;
			if (temp.charAt(0)=='-')
				objeto.value = temp.substring(1, temp.length-1);
			else
				objeto.value = '-' + temp;
		}
		if (key==44 && __textbox.hasPoint(objeto.value)) key=14;
		return (key <= 13 || key == 44 || (key >= 48 && key <= 57));
	}
	function OnlyNumbers(evt){
		// NOTA: borrar = 8, Intro = 13, '0' = 48, '9' = 57	
		var nav4 = window.Event ? true : false;
		var key = nav4 ? evt.which : event.keyCode;
		return (key <= 13 || (key >= 48 && key <= 57));
	}
	function OnlySignedNumbers(evt, objeto){
		// NOTA: borrar = 8, Intro = 13, '0' = 48, '9' = 57	
		var nav4 = window.Event ? true : false;
		var key = nav4 ? evt.which : event.keyCode;
		if (key == 45){
			var temp = objeto.value;
			if (temp.charAt(0)=='-')
				objeto.value = temp.substring(1, temp.length-1);
			else
				objeto.value = '-' + temp;
		}
		return (key <= 13 || (key >= 48 && key <= 57));
	}
	function CheckLength(evt, obj, max)
	{
		var nav4 = window.Event ? true : false;
		var key = nav4 ? evt.which : event.keyCode;
		if (obj.value.length>=max)
			return (key <= 13);
		return true;
	}
}