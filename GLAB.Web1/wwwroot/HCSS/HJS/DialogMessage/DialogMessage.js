async function HideDialogMessage(){
    
    var dialogMessageContainer=document.getElementById("dialogMessageContainer");
    if(dialogMessageContainer === undefined) return;
    
    dialogMessageContainer.classList.remove("dialogMessageContainerShown");
    dialogMessageContainer.classList.add("dialogMessageContainerHidden");
    
}

setTimeout(HideDialogMessage,2500);