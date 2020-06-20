

function iFrameOn() {
    richTextField.document.designMode = 'On';
}
function iBold() {
    richTextField.document.execCommand('bold', false,null);
}
function Undeline() {
    richTextField.document.execCommand('underline', false, null);
}
function iItalic() {
    richTextField.document.execCommand('italic', false, null);
}
function iFontSize() {
    var size = prompt('Enter a size 1-7', '');
    richTextField.document.execCommand('FontSize', false, size);
}
function iForeColor() {
    var color = prompt('Define a basic color or apply a hexadecimal color code for advanced colors:', '');
    richTextField.document.execCommand('ForeColor', false, color);
}
function iHorizontalRule() {
    richTextField.document.execCommand('inserthorizontalrule', false, null);
}
function iUnorderedList() {
    richTextField.document.execCommand('InsertOrderedList', false, 'newUL');
}
function iOrderedList() {
    richTextField.document.execCommand('InsertUnorderedList', false, 'newOL');
}
function iLink() {
    var link = prompt('Enter the URL for required link', 'http://');
    richTextField.document.execCommand('CreateLink', false, link);
}
function iUnlink() {
    richTextField.document.execCommand('Unlink', false, link);
}

function submit_form() {
    var theForm = document.getElementById("myform");
    theForm.elements["myTextArea"].value = window.frames['richTextField'].document.body.innerHtml;
    theForm.submit();
    alert("help");
}

