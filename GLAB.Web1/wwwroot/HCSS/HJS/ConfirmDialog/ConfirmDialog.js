function hideConfirmDialog(){
    var confirmDialog=document.getElementById("confirmDialog");
    if(confirmDialog!==null && confirmDialog !== undefined){
        confirmDialog.classList.remove("confirmDialogShown");
        confirmDialog.classList.add("confirmDialogHidden")
    }
}