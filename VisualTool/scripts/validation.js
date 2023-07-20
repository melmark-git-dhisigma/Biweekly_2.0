function validate(controlName, checkFor) { //required,email,numbers,texts

    var controlValue = $(controlName).val();
    var valid = true;
    var errorMsg = "";

    //required
    if (controlValue == "") {
        errorMsg = "Field is empty";
    }
}