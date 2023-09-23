import { Game } from "./Classes/Game.js";

let adminUsername = sessionStorage.getItem("usernameAdmin");
if(adminUsername == null)
{
    document.body.style.visibility = "hidden";
}

document.addEventListener("DOMContentLoaded", () => {
    const addForm = document.querySelector("#addForm");

    addForm.addEventListener("submit", e => {
        e.preventDefault();

        var img = addForm.gameImg.files[0];
        console.log(img);
        var name = addForm.gameName.value;
        var description = addForm.gameDescription.value;
        var developer = addForm.gameDeveloper.value;
        var publisher = addForm.gamePublisher.value;
        var genres = addForm.gameGenres.value;
        var systems = addForm.gameSystems.value;
        var year = addForm.gameYear.value;
        var price = addForm.gamePrice.value;
        var rating = addForm.gameRating.value;

        let formImg = new FormData();
        formImg.append('img', img);

        fetch('https://localhost:7067/Game/SaveImage',
        {
            method: "POST",
            body: formImg
        }).then(p => {
            if(p.ok){
                p.text().then(pData => {
                    console.log(pData);
                    let imgPath = pData;
                    addGame(imgPath,name,description,developer,publisher,genres,systems,year,price,rating);
                    alert("Game added successfully!");
                })
                window.location.replace("./homepageAdmin.html");
                }
            else
            {
                alert("Error while saving image!");
            }    
            }) 
    })
})

function addGame(img,nam,desc,dev,pub,gen,sys,yr,pr,rt)
{
    fetch('https://localhost:7067/Game/AddGame',
        {
            method: "POST",
            body: JSON.stringify({image:img,name:nam,description:desc,developer:dev,publisher:pub,genres:gen,systems:sys,year:yr,price:pr,rating:rt}),
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(s => {
            if(s.ok){
                console.log("Game added successfully!");
            }
            else
            {
                alert("Error while adding game!");
            }
        })
}