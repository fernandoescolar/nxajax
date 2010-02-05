var __HTMLEditor = new nxHTMLEditor();

function nxHTMLEditor ()
{
	this.instanceName = "__HTMLEditor";
	this.mSmilesFilePath = "resources/addsmile.php";
	this.mTableFilePath = "resources/MakeTable.aspx";
	this.mIconsPath = "images/richtextbox";

	this.Render = Render;
	this.doCommand = doCommand;
	this.showColors = showColors;
	this.Set = Set;
	this.Blur = Blur;
	this.selOn = selOn;
	this.selOff = selOff;
	this.selDown = selDown;
	this.selUp = selUp;
	this.Load = Load;
	this.FormatText = FormatText;
	
	
	init();

	function FormatText(command, UI, option, name){
		var obj = (!document.all)?document.getElementById('HTMLEditControl_' + name).contentWindow:frames['HTMLEditControl_' + name];
		obj.focus();
		obj.document.execCommand(command, UI, option);
		obj.focus();
	}
	function doCommand(ctrl, name){
			switch(ctrl.id.replace("_" + name, "")){
				case 'imgInsertTable' :
					window.open(this.mTableFilePath + '?name=' + name, null, 'top=50,left=100,width=300,height=200,location=no,status=yes,menubar=no,resizable=yes,scrollbars=auto');
					break;
				case 'imgFile' :
					window.open('nxFiles.aspx?action=showmyfiles&name=' + name, null, 'top=50,left=100,width=300,height=300,location=no,status=yes,menubar=no,resizable=yes,scrollbars=auto');
					break;
				case 'imgImage' :
					if (ns6){
						var imagePath = prompt('Introduce la URL', 'http://');
						if ((imagePath != null) && (imagePath != ''))
							this.FormatText('insertimage', false, imagePath, name);
					}else{
						this.FormatText('insertimage', true, '', name);
					}
					break;
				case 'imgLink' :
					if (ns6){
						var imagePath = prompt('Introduce la URL', 'http://');
						if ((imagePath != null) && (imagePath != ''))
							this.FormatText('createlink', false, imagePath, name);
					}else{
						this.FormatText('createlink', true, '', name);
					}
					break;
				case 'lstStyle' :
					this.FormatText('formatblock', false, ctrl.options[ctrl.selectedIndex].value, name);
					break;
				case 'lstFontSize' :
					this.FormatText('fontsize', false, ctrl.options[ctrl.selectedIndex].value, name);
					break;
				case 'lstFont' :
					this.FormatText('fontname', false, ctrl.options[ctrl.selectedIndex].value, name);
					break;
				case 'lstColor' :
					this.FormatText('fontforecolorname', false, ctrl.options[ctrl.selectedIndex].value, name);
					break;
				case 'imgCustom' :
					var temp = (ns6) ? document.getElementById('table2_' + name).style.display:eval('table2_' + name).style.display;
					var mTexto = ns6 ? document.getElementById('HTMLEdit2Control_' + name) : document.all['HTMLEdit2Control_' + name];
					var mEditor = ns6 ? document.getElementById('HTMLEditControl_' + name) : frames['HTMLEditControl_' + name];
					if (temp == 'none')
					{
						var mTemp1 = mTexto.value;
						mEditor.document.body.innerHTML = mTemp1;
						mTexto.style.display = 'none';
						(ns6) ? document.getElementById('table1_' + name).style.display = 'inline' : eval ('table1_' + name).style.display = 'inline';
						(ns6) ? document.getElementById('table2_' + name).style.display = 'inline' : eval ('table2_' + name).style.display = 'inline';
						(document.getElementById) ? document.getElementById('HTMLEditControl_' + name).style.display = 'inline' : frames['HTMLEditControl_' + name].style.display = 'inline';
					}
					else
					{
						var mTemp = mEditor.document.body.innerHTML;
						mTexto.value = mTemp;
						mTexto.style.zorder = 0;
						(ns6) ? document.getElementById('table1_' + name).style.display = 'none' : eval ('table1_' + name).style.display = 'none';
						(ns6) ? document.getElementById('table2_' + name).style.display = 'none' : eval ('table2_' + name).style.display = 'none';
						(document.getElementById) ? document.getElementById('HTMLEditControl_' + name).style.display = 'none': frames['HTMLEditControl_' + name].style.display = 'none';
						mTexto.style.display = 'inline';
					}
					break;
				case 'imgSmile' :
					window.open(this.mSmilesFilePath, null, 'top=50,left=100,width=300,height=300,location=no,status=yes,menubar=no,resizable=yes,scrollbars=auto');
				break;
			default:
				var temp = ctrl.id.replace("_" + name, "").toLowerCase();
				var mCommand = temp.substring(3, temp.length);
				this.FormatText(mCommand, false, '', name);
				break;
		}
	}
	function GetEditBoxColor(colorCommand, name){
		return DecimalToRGB(frames['HTMLEditControl_' + name].document.queryCommandValue(colorCommand));
	}
	function DecimalToRGB(value) {
		var hex_string = '';
		for (var hexpair = 0; hexpair < 3; hexpair++) {
			var bbyte = value & 0xFF;            /* get low byte*/
			value >>= 8;                        /* drop low byte*/
			var nybble2 = bbyte & 0x0F;          /* get low nybble (4 bits)*/
			var nybble1 = (bbyte >> 4) & 0x0F;   /* get high nybble*/
			hex_string += nybble1.toString(16); /* convert nybble to hex*/
			hex_string += nybble2.toString(16); /* convert nybble to hex*/
		}
		return hex_string.toUpperCase();
	}
	function showColors(evento, bShow, name) {
		//if (!ns6) {
		//	var oldcolor = GetEditBoxColor('forecolor', name);
		//	var vValue = GetColorFromUser(oldcolor);
		//	this.FormatText(evento, false, vValue, name);
		//} else {
			var obj = document.getElementById('PickColor_' + name + "_" + evento);
			obj.style.display = bShow ? 'block':'none';
		//}
	}
	function Set(string, id, name) {                   /* select color*/
		var color = ValidateColor(string);
		if (color == null) { alert('codigo de color invalido: ' + string); }        /* invalid color*/
		else {
		    if (!document.all)                                                                /* valid color*/
			    this.FormatText(id, false, color, name);                          /* show selected color*/
			else
			{
			    var obj = frames['HTMLEditControl_' + name];
			    var oSel = obj.document.selection.createRange();
			    if (oSel.text != '')
			    {
			        oSel.select();
			    }
			    else
			        alert('select some text');
			    
			    obj.focus();
			    obj.document.execCommand(id, false, '#' + color);
			    obj.focus();
			}
			showColors(id, false, name);
		}
	}
	function ValidateColor(string) {                /* return valid color code*/
		string = string || '';
		string = string + '';
		string = string.toUpperCase();
		chars = '0123456789ABCDEF';
		out   = '';
		for (i=0; i<string.length; i++) {             /* remove invalid color chars*/
			schar = string.charAt(i);
			if (chars.indexOf(schar) != -1) { out += schar; }
		}
		if (out.length != 6) { return null; }            /* check length*/
		return out;
	}
//	function GetColorFromUser(oldcolor){
//		var posX    = event.screenX;
//		var posY    = event.screenY + 20;
//		var screenW = screen.width;                                 /* screen size*/
//		var screenH = screen.height - 20;                           /* take taskbar into account*/
//		if (posX + 232 > screenW) { posX = posX - 232 - 40; }       /* if mouse too far right*/
//		if (posY + 164 > screenH) { posY = posY - 164 - 80; }       /* if mouse too far down*/
//		var wPosition   = 'dialogLeft:' +posX+ '; dialogTop:' +posY;
//		var newcolor = showModalDialog(this.mColorFilePath, oldcolor,
//			'dialogWidth:238px; dialogHeight: 187px; '
//			+ 'resizable: no; help: no; status: no; scroll: no; '
//			+ wPosition);
//		return newcolor;
//	}
	function Blur(name) { try{ var temp = (ns6) ? document.getElementById("table2_" + name).style.display : eval("table2_" + name +".style.display"); var mTexto = (ns6) ? document.getElementById(name):eval("document.all." + name); var mEditor = (ns6) ? document.getElementById('HTMLEditControl_' + name).contentWindow : frames['HTMLEditControl_' + name]; var mEditor2 = (ns6) ? document.getElementById('HTMLEdit2Control_' + name).contentWindow:document.all['HTMLEdit2Control_' + name]; var mTemp1=''; if (temp == 'none' ) mTemp1 = mEditor2.value; else mTemp1 = mEditor.document.body.innerHTML; mTexto.value = mTemp1; } catch(ex){} setTimeout(this.instanceName + '.Blur("' + name + '")', 200); } 
	function selOn(ctrl) { ctrl.className = 'butClass2' }
	function selOff(ctrl) { ctrl.className = 'butClass' }
	function selDown(ctrl) { ctrl.className='butClass3' } function selUp(ctrl) { ctrl.className='butClass'}
	function Load(name) { 
		/*try { 
			var mObj = (ns6)?document.getElementById('HTMLEditControl_' + name).contentWindow:frames['HTMLEditControl_' + name]; 
			var mTexto = ns6 ? document.getElementById(name) : document.all[name];
			mObj.document.innerHTML = mTexto.value;
			//alert(mTexto.value);
		} catch(ex){//alert('error: ' + ex.message);} */
		setTimeout(this.instanceName + '.Blur("' + name + '")', 200);
	}
	
	function init()
	{
		this.instanceName = "__HTMLEditor";
		//this.mColorFilePath = "resources/ColorPicker.html";
		this.mSmilesFilePath = "resources/addsmile.php";
		this.mTableFilePath = "resources/MakeTable.htm";
		this.mIconsPath = "images/richtextbox";

		this.ie4 = document.all && navigator.userAgent.indexOf("Opera")==-1;
		this.ns6 = document.getElementById&&!document.all;
	}
	
	function CreateColorPalatte(name, id)
	{
		var mStr, ieAdd;
		ieAdd = (document.all)?'><img' : '';
		    
		mStr = "<DIV id='PickColor_" + name + "_" + id + "' style='display: none'>";
		mStr += "<table border=0 cellspacing=1 cellpadding=0 style='cursor: hand;width: 100%;'>" ;
		mStr += "   <tr>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #000000' onClick=\"" + this.instanceName + ".Set('000000', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FFFFFF' onClick=\"" + this.instanceName + ".Set('FFFFFF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #0000FF' onClick=\"" + this.instanceName + ".Set('0000FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00FF00' onClick=\"" + this.instanceName + ".Set('00FF00', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF0000' onClick=\"" + this.instanceName + ".Set('FF0000', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF00FF' onClick=\"" + this.instanceName + ".Set('FF00FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00FFFF' onClick=\"" + this.instanceName + ".Set('00FFFF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FFFF00' onClick=\"" + this.instanceName + ".Set('FFFF00', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #3300FF' onClick=\"" + this.instanceName + ".Set('3300FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF0033' onClick=\"" + this.instanceName + ".Set('FF0033', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #0033FF' onClick=\"" + this.instanceName + ".Set('0033FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00FF33' onClick=\"" + this.instanceName + ".Set('00FF33', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #33FF00' onClick=\"" + this.instanceName + ".Set('33FF00', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF3300' onClick=\"" + this.instanceName + ".Set('FF3300', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #333333' onClick=\"" + this.instanceName + ".Set('333333', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #6600FF' onClick=\"" + this.instanceName + ".Set('6600FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF0066' onClick=\"" + this.instanceName + ".Set('FF0066', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #0066FF' onClick=\"" + this.instanceName + ".Set('0066FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00FF66' onClick=\"" + this.instanceName + ".Set('00FF66', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #66FF00' onClick=\"" + this.instanceName + ".Set('66FF00', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF6600' onClick=\"" + this.instanceName + ".Set('FF6600', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #666666' onClick=\"" + this.instanceName + ".Set('666666', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #9900FF' onClick=\"" + this.instanceName + ".Set('9900FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF0099' onClick=\"" + this.instanceName + ".Set('FF0099', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #0099FF' onClick=\"" + this.instanceName + ".Set('0099FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00FF99' onClick=\"" + this.instanceName + ".Set('00FF99', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #99FF00' onClick=\"" + this.instanceName + ".Set('99FF00', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF9900' onClick=\"" + this.instanceName + ".Set('FF9900', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #999999' onClick=\"" + this.instanceName + ".Set('999999', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #CC00FF' onClick=\"" + this.instanceName + ".Set('CC00FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF00CC' onClick=\"" + this.instanceName + ".Set('FF00CC', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00CCFF' onClick=\"" + this.instanceName + ".Set('00CCFF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00FFCC' onClick=\"" + this.instanceName + ".Set('00FFCC', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #CCFFCC' onClick=\"" + this.instanceName + ".Set('CCFFCC', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FFCCCC' onClick=\"" + this.instanceName + ".Set('FFCCCC', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #CCCCCC' onClick=\"" + this.instanceName + ".Set('CCCCCC', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		if (document.all)
		{
		    mStr += "       <td class='tdClass' align=right><img ID='imgClose' alt='Cerrar' name='imgClose' border='0' src='" + this.mIconsPath + "/close.gif' onClick=\"" + this.instanceName + ".showColors('" + id + "', false, '" + name + "')\" /></td>" ;
		    mStr += "       </tr><tr>" ;
		}
		
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #000000' onClick=\"" + this.instanceName + ".Set('000000', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FFFFFF' onClick=\"" + this.instanceName + ".Set('FFFFFF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #0000FF' onClick=\"" + this.instanceName + ".Set('0000FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00FF00' onClick=\"" + this.instanceName + ".Set('00FF00', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF0000' onClick=\"" + this.instanceName + ".Set('FF0000', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF00FF' onClick=\"" + this.instanceName + ".Set('FF00FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00FFFF' onClick=\"" + this.instanceName + ".Set('00FFFF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FFFF00' onClick=\"" + this.instanceName + ".Set('FFFF00', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #3300FF' onClick=\"" + this.instanceName + ".Set('3300FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF0033' onClick=\"" + this.instanceName + ".Set('FF0033', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #0033FF' onClick=\"" + this.instanceName + ".Set('0033FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00FF33' onClick=\"" + this.instanceName + ".Set('00FF33', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #33FF00' onClick=\"" + this.instanceName + ".Set('33FF00', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF3300' onClick=\"" + this.instanceName + ".Set('FF3300', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #333333' onClick=\"" + this.instanceName + ".Set('333333', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #6600FF' onClick=\"" + this.instanceName + ".Set('6600FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF0066' onClick=\"" + this.instanceName + ".Set('FF0066', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #0066FF' onClick=\"" + this.instanceName + ".Set('0066FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00FF66' onClick=\"" + this.instanceName + ".Set('00FF66', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #66FF00' onClick=\"" + this.instanceName + ".Set('66FF00', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF6600' onClick=\"" + this.instanceName + ".Set('FF6600', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #666666' onClick=\"" + this.instanceName + ".Set('666666', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #9900FF' onClick=\"" + this.instanceName + ".Set('9900FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF0099' onClick=\"" + this.instanceName + ".Set('FF0099', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #0099FF' onClick=\"" + this.instanceName + ".Set('0099FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00FF99' onClick=\"" + this.instanceName + ".Set('00FF99', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #99FF00' onClick=\"" + this.instanceName + ".Set('99FF00', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF9900' onClick=\"" + this.instanceName + ".Set('FF9900', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #999999' onClick=\"" + this.instanceName + ".Set('999999', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #CC00FF' onClick=\"" + this.instanceName + ".Set('CC00FF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FF00CC' onClick=\"" + this.instanceName + ".Set('FF00CC', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00CCFF' onClick=\"" + this.instanceName + ".Set('00CCFF', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #00FFCC' onClick=\"" + this.instanceName + ".Set('00FFCC', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #CCFFCC' onClick=\"" + this.instanceName + ".Set('CCFFCC', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #FFCCCC' onClick=\"" + this.instanceName + ".Set('FFCCCC', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td class='tdClass'" + ieAdd + " style='background-color: #CCCCCC' onClick=\"" + this.instanceName + ".Set('CCCCCC', '" + id + "', '" + name + "')\" onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' height=10 width=10></td>" ;
		mStr += "       <td><td>";
		
		if (!document.all)
		    mStr += "       <td class='tdClass' align=right><img ID='imgClose' alt='Cerrar' name='imgClose' border='0' src='" + this.mIconsPath + "/close.gif' onClick=\"" + this.instanceName + ".showColors('" + id + "', false, '" + name + "')\" /></td>" ;
		    
		mStr += "       </tr>" ;
		mStr += "   </table>" ;
		mStr += "</DIV>" ;

		return mStr;
	}

	function GenerateHTMLAreaBody(name, text)
	{
		var mStr = '';
		//Start of Main Table which will have three tables
		mStr = "<INPUT type='hidden' id='txtOperation' name='txtOperation'>"
		mStr += "<TABLE align='center' class='tblTable' cellpadding=0 cellspacing=0>" ;
		mStr += "<TR>" ;
		mStr += "<TD class='tdClass'>" ;

		//Start Making First Table (ie First Row of Toolbar) ;
		mStr += "			<TABLE id='table1_" + name + "' class='tblTable'>" ;
		mStr += "				<TR>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgUndo' alt='Deshacer' name='_" + name + "imgUndo' class='butClass' src='" + this.mIconsPath + "/undo.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgRedo' alt='Rehacer' name='_" + name + "imgRedo' class='butClass' src='" + this.mIconsPath + "/redo.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgSubScript' alt='Subíndice' name='_" + name + "imgSubScript' class='butClass' src='" + this.mIconsPath + "/subscript.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgSuperScript' alt='Superíndice' name='_" + name + "imgSuperScript' class='butClass' src='" + this.mIconsPath + "/superscript.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgInsertOrderedList' alt='Lista Ordenada' name='_" + name + "imgInsertOrderedList' class='butClass' src='" + this.mIconsPath + "/orderedlist.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgInsertUnOrderedList' alt='Lista sin Orden' name='_" + name + "imgInsertUnOrderedList' class='butClass' src='" + this.mIconsPath + "/unorderedlist.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img alt='Outdent' ID='_" + name + "imgOutdent' name='_" + name + "Quitar Sangrado' class='butClass' src='" + this.mIconsPath + "/outdent.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img alt='Indent' ID='_" + name + "imgIndent' name='_" + name + "Añadir Sangrado' class='butClass' src='" + this.mIconsPath + "/indent.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'> <!-- Insert the Style List -->" ;
		mStr += "						<select id='_" + name + "lstStyle' width=30px onChange='" + this.instanceName + ".doCommand(this, \"" + name + "\");'>" ;
		mStr += "							<option value='paragraph'>Parrafo</option>" ;
		mStr += "							<option value='Heading 1'>H1</option>" ;
		mStr += "							<option value='Heading 2'>H2</option>" ;
		mStr += "							<option value='Heading 3'>H3</option>" ;
		mStr += "							<option value='Heading 4'>H4</option>" ;
		mStr += "							<option value='Heading 5'>H5</option>" ;
		mStr += "							<option value='Heading 6'>H6</option>" ;
		mStr += "							<option value='Heading 7'>H7</option>" ;
		mStr += "						</select>" ;
		mStr += "					</td>" ;
		mStr += "					<td class='tdClass'>" ;
		mStr += "						<Select id='_" + name + "lstFont' name='lstFont' width=30px onChange='" + this.instanceName + ".doCommand(this, \"" + name + "\");'>" ;
		mStr += "							<option value='Arial'>Arial</option>" ;
		mStr += "							<option value='Courier'>Courier</option>" ;
		mStr += "							<option value='Sans Serif'>Sans Serif</option>" ;
		mStr += "							<option value='Tahoma'>Tahoma</option>" ;
		mStr += "							<option value='Verdana'>Verdana</option>" ;
		mStr += "							<option value='Wingdings'>Wingdings</option>" ;
		mStr += "						</Select>" ;
		mStr += "					</td>" ;
		mStr += "					<td class='tdClass'>" ;
		mStr += "						<select id='_" + name + "lstFontSize' onChange='" + this.instanceName + ".doCommand(this, \"" + name + "\");' width=30px>" ;
		mStr += "							<option value=1>Muy Peque&ntilde;a</option>" ;
		mStr += "							<option value=2>Peque&ntilde;a</option>" ;
		mStr += "							<option value=3>Mediana</option>" ;
		mStr += "							<option value=4>Grande</option>" ;
		mStr += "							<option value=5>Mas Grande</option>" ;
		mStr += "							<option value=6>Muy Grande</option>" ;
		mStr += "							<option value=7>Extra Grande</option>" ;
		mStr += "						</select>" ;
		mStr += "					</td>" ;
		mStr += "				</TR>" ;
		mStr += "			</TABLE>" ;

		//End of First Table
		mStr += "</TD>" ;
		mStr += "</TR>" ;

		//Create Second Table Now
		mStr += "	<TR>" ;
		mStr += "		<TD class='tdClass'>" ;
		mStr += "			<TABLE id='table2_" + name + "' class='tblTable'>" ;
		mStr += "				<TR>" ;
		mStr += "					<TD class='tdClass'>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgBold' alt='Negrita' name='_" + name + "imgBold' class='butClass' src='" + this.mIconsPath + "/bold.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgItalic' alt='Cursiva' name='_" + name + "imgItalic' class='butClass' src='" + this.mIconsPath + "/italic.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgUnderLine' alt='Subrayar' name='_" + name + "imgUnderLine' class='butClass' src='" + this.mIconsPath + "/underline.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgStrikeThrough' alt='Tachar' name='_" + name + "imgStrikeThrough' class='butClass' src='" + this.mIconsPath + "/strikethrough.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img src='" + this.mIconsPath + "/separator.gif'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgJustifyLeft' alt='Alinear Izquierda' name='_" + name + "imgJustifyLeft' class='butClass' src='" + this.mIconsPath + "/alignleft.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgJustifyCenter' alt='Centrar' name='_" + name + "imgJustifyCenter' class='butClass' src='" + this.mIconsPath + "/aligncenter.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgJustifyRight' alt='Alinear Derecha' name='_" + name + "imgJustifyRight' class='butClass' src='" + this.mIconsPath + "/alignright.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgJustifyFull' alt='Justificado' name='_" + name + "imgJustifyFull' class='butClass' src='" + this.mIconsPath + "/alignjustify.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img src='" + this.mIconsPath + "/separator.gif'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgCut' alt='Cortar' name='_" + name + "imgCut' class='butClass' src='" + this.mIconsPath + "/cut.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgCopy' alt='Copiar' name='_" + name + "imgCopy' class='butClass' src='" + this.mIconsPath + "/copy.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgPaste' alt='Pegar' name='_" + name + "imgPaste' class='butClass' src='" + this.mIconsPath + "/paste.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img src='" + this.mIconsPath + "/separator.gif'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgImage' alt='Insertar Imagen' name='_" + name + "imgImage' class='butClass' src='" + this.mIconsPath + "/image.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgLink' alt='HyperLink' name='_" + name + "imgLink' class='butClass' src='" + this.mIconsPath + "/link.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='doCommand(this)'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgInsertHorizontalRule' alt='Insertar Linea' name='_" + name + "imgInsertHorizontalRule' class='butClass' src='" + this.mIconsPath + "/line.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgInsertTable' alt='Insertar Tabla' name='_" + name + "imgInsertTable' class='butClass' src='" + this.mIconsPath + "/table.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgFontColor' alt='Color del Texto' name='_" + name + "imgFontColor' class='butClass' src='" + this.mIconsPath + "/fontcolor.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick=\"" + this.instanceName + ".showColors('forecolor', true, '" + name + "')\"></td>" ;
		mStr += "					<td class='tdClass'><img ID='_" + name + "imgHighLight' alt='Color de Fondo' name='_" + name + "imgHighLight' class='butClass' src='" + this.mIconsPath + "/highlight.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick=\"" + this.instanceName + ".showColors('backcolor', true, '" + name + "')\"></td>" ;
		//mStr += "					<td class='tdClass'><img ID='_" + name + "imgFile' alt='Insertar Archivo' name='_" + name + "imgFile' class='butClass' src='" + this.mIconsPath + "/upload.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'></td>" ;
		mStr += "				</TR>" ;
		mStr += "			</TABLE>" ;
		//End of Second Table

		mStr += "		</TD>" ;
		mStr += "</TR>" ;
		mStr += "	<TR>" ;
		mStr += "		<TD class='tdClass'>" ;
		
		//if (this.ns6)
		{
			mStr += CreateColorPalatte(name, "forecolor");
			mStr += CreateColorPalatte(name, "backcolor");
		}
		
		mStr += "		</TD>" ;
		mStr += "</TR>" ;

		//Create Third Table Now
		mStr += "	<TR>" ;
		mStr += "		<TD class='tdClass'>" ;
		mStr += "			<TABLE id='table3' class='tblTable'>" ;
		mStr += "				<TR>" ;
		mStr += "					<TD class='tdClass'>" ;
		mStr += "							<INPUT name='" + name + "' ID='" + name + "' style='display: none;'>";
		mStr += "							<IFrame OnLoad='" + this.instanceName + ".Load(\"" + name + "\");' name='HTMLEditControl_" + name + "' ID='HTMLEditControl_" + name + "' class='EditControl' ></IFrame>" ;
		mStr += "							<TEXTAREA name='HTMLEdit2Control_" + name + "' ID='HTMLEdit2Control_" + name + "' class='EditControl2' ></TEXTAREA>" ;
		mStr += "					</TD>" ;
		mStr += "				</TR>" ;
		mStr += "				<TR>" ;
		mStr += "					<td align='right' class='tdClass'>" ;
		mStr += "						<img alt='HTML View' ID='imgCustom' name='imgCustom' class='butClass' src='" + this.mIconsPath + "/customtag.gif' onMouseOver='" + this.instanceName + ".selOn(this)' onMouseOut='" + this.instanceName + ".selOff(this)' onMouseDown='" + this.instanceName + ".selDown(this)' onMouseUp='" + this.instanceName + ".selUp(this)' onClick='" + this.instanceName + ".doCommand(this, \"" + name + "\")'>" ;
		mStr += "					</td>" ;
		mStr += "				</TR>" ;
		mStr += "			</TABLE>" ;
		mStr += "		</TD>" ;
		mStr += "	</TR>" ;
		mStr += "</TABLE>" ;

		//That's all, Now return the HTML String
		return (mStr);
	}
	function Write(id, msg)
	{
		var d = $(id);
		
		if(!d)
		{
			document.write(msg);
		}
		else
		{
			d.innerHTML += msg; 
		}
	}
	function Render(id, name, text)
	{
			text = text.replace("'", '"');
			Write(id, GenerateHTMLAreaBody(name, text));
			//Conver the Simple IFrame to Editable TextBox
			//frames['HTMLEditControl_' + name].document.designMode='on';
			var obj = (document.getElementById)?document.getElementById('HTMLEditControl_' + name).contentWindow:frames['HTMLEditControl_' + name];
			obj.document.designMode = 'On';
			if (ns6)
			{
				obj.document.open();
				if (text == '')
					obj.document.write ('< html >');
			
				obj.document.close ();
				FormatText('backcolor', false, 'white', name);
			}
			obj.document.designMode = 'On';
			var mTexto = (document.getElementById) ? document.getElementById(name) : document.all[name];
			mTexto.value = text;
			var mObj = (document.getElementById)?document.getElementById('HTMLEditControl_' + name).contentWindow:frames['HTMLEditControl_' + name]; 
			var rObj = mObj.document;
			rObj.open();
			rObj.write(mTexto.value);
			rObj.close();
			
	}
}

function RenderHTMLEditor(divId, name, text)
{
	if(!text)
		text='';
	__HTMLEditor.Render(divId, name, text);
}