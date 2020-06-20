function ValidateEmail() {
    var email = document.getElementById("useEmailAddress").value;
    var lblError = document.getElementById("lblError");
    lblError.innerHTML = "";
    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (!expr.test(email)) {
        lblError.innerHTML = "Invalid email address.";
        document.getElementById("btnSubmit").disabled = true;
    }
    else {
        document.getElementById("btnSubmit").disabled = false;
    }
}