# nxajax

An easy use asp.net jquery ajax library using jQuery

Learn a quick example: [Quick Example] (with MainPage and ContainedPage) and [Ajax Form Example] (testing)

*Features:*
* jQuery javascript core
* Ajax WebControl PostbackModes: Async or Sync.
** AjaxQueue (by Pat Nakajima) used by Sync PostBacks
** Auto "loading message" in Sync PostBacks (in MainPage div -> id="loading")
** Auto Loading Image in Async PostBacks (each control: LoadingImg or LoadingImgId attribute)
* Use it in your existing WebForms by adding an ajax form control
* MainPage or ajax Form Control with container, loads ContainedPages.
* Multilanguage System
* Template based Web Render
* Low bandwidth waste: Only post the current control value
** Only post all form content on ISubmit control is clicked
* [DOCTYPE XHTML 1.1 W3C compliant|Why DOCTYPE XHTML1.1?]

*Included ajax web controls:*
* Autocomplete TextBox (not yet)
* Button
* ComboBox
* Container (loads nxContainedPages)
* CheckBox
* DatePicker (DatePicker  jquery plugin)
* DragPanel, DropPanel and DragnDropManager (like iGoogle jQuery UI)
* EditableComboLabel (label becomes ComboBox)
* EditableLabel (label becomes TextBox)
* EditableTextAreaLabel (label becomes TextArea)
* FileUpload (AJAX Upload jquery plugin)
* GridView
* Hidden
* ImageButton
* InputImage
* Label
* LinkButton
* LinkSubmit
* Menu
* Pager
* Radio
* RichTextArea (WYSIWYG jquery plugin)
* Submit
* TextArea
* TextBox
* TreeView (Treeview jquery plugin)

----
nxAjax uses:

!!!! Omar Al Zabir - HTTP Handler to Combine Multiple Files, Cache and Deliver Compressed Output for Faster Page Load
[url:http://www.codeproject.com/KB/aspnet/HttpCombine.aspx]

!!!! Mono System.Web.HtmlTextWriter
[url:http://www.mono-project.com/]

!!!! jQuery JavaScript Library v1.3.2
[url:http://jquery.com/]
Copyright (c) 2009 John Resig
Dual licensed under the MIT and GPL licenses.
[url:http://docs.jquery.com/License]

!!!! jQuery UI 1.7.2
Copyright (c) 2009 AUTHORS.txt ([url:http://jqueryui.com/about])
Dual licensed under the MIT (MIT-LICENSE.txt)
and GPL (GPL-LICENSE.txt) licenses.
[url:http://docs.jquery.com/UI]

!!!! AjaxQueue - by Pat Nakajima

!!!! AJAX Upload
Project page - [url:http://valums.com/ajax-upload/]
Copyright (c) 2008 Andris Valums, [url:http://valums.com]
Licensed under the MIT license ([url:http://valums.com/mit-license/])

!!!! Cookie plugin
Copyright (c) 2006 Klaus Hartl (stilbuero.de)
Dual licensed under the MIT and GPL licenses:
[url:http://www.opensource.org/licenses/mit-license.php]
[url:http://www.gnu.org/licenses/gpl.html]

!!!! Datepicker for jQuery 3.7.0.
[url:http://keith-wood.name/datepick.html]
Written by Marc Grabanski (m@marcgrabanski.com) and
Keith Wood (kbwood{at}iinet.com.au).
Dual licensed under the GPL ([url:http://dev.jquery.com/browser/trunk/jquery/GPL-LICENSE.txt]) and 
MIT ([url:http://dev.jquery.com/browser/trunk/jquery/MIT-LICENSE.txt]) licenses. 

!!!! WYSIWYG - jQuery plugin 0.5
Copyright (c) 2008-2009 Juan M Martinez
[url:http://plugins.jquery.com/project/jWYSIWYG]
Dual licensed under the MIT and GPL licenses:
[url:http://www.opensource.org/licenses/mit-license.php]
[url:http://www.gnu.org/licenses/gpl.html]

!!!! Treeview 1.4 - jQuery plugin to hide and show branches of a tree
[url:http://bassistance.de/jquery-plugins/jquery-plugin-treeview/]
[url:http://docs.jquery.com/Plugins/Treeview]
Copyright (c) 2007 Jörn Zaefferer
Dual licensed under the MIT and GPL licenses:
[url:http://www.opensource.org/licenses/mit-license.php]
[url:http://www.gnu.org/licenses/gpl.html]
