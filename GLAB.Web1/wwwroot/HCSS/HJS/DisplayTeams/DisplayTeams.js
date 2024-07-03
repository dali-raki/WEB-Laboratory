
async function displayDetailedTeamCard(card){
    var teamCardBaseInfo=document.querySelector(".teamCardBaseInfo");
    card.classList.add("rotated");
    
    await new Promise(resolve => setTimeout(resolve,300));
        
    card.classList.remove("teamCardContainerBaseInfo");
}



async function HideAddTeamPopup(){
    var addTeamPopupContent=document.getElementById("addTeamPopupContent");
    if(addTeamPopupContent === undefined) return;
    
    addTeamPopupContent.classList.remove("addTeamPopupContentShown");
    addTeamPopupContent.classList.add("addTeamPopupContentHidden");
}