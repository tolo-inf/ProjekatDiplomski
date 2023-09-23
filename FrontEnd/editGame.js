import { Game } from "./Classes/Game.js";

let adminUsername = sessionStorage.getItem("usernameAdmin");
if(adminUsername == null)
{
    document.body.style.visibility = "hidden";
}

let gameId = localStorage.getItem("gameId");
let gameImg = localStorage.getItem("gameImg");
let gameName = localStorage.getItem("gameName");
let gameDescription = localStorage.getItem("gameDescription");
let gameDeveloper = localStorage.getItem("gameDeveloper");
let gamePublisher = localStorage.getItem("gamePublisher");
let gameGenres = localStorage.getItem("gameGenres");
let gameSystems = localStorage.getItem("gameSystems");
let gameYear = localStorage.getItem("gameYear");
let gamePrice = localStorage.getItem("gamePrice");
let gameRating = localStorage.getItem("gameRating");

let inName = document.getElementById("gameName");
inName.value = gameName;
inName.disabled = true;
let inDescription = document.getElementById("gameDescription");
inDescription.value = gameDescription;
let inDeveloper = document.getElementById("gameDeveloper");
inDeveloper.value = gameDeveloper;
let inPublisher = document.getElementById("gamePublisher");
inPublisher.value = gamePublisher;
let inGenres = document.getElementById("gameGenres");
inGenres.value = gameGenres;
let inSystems = document.getElementById("gameSystems");
inSystems.value = gameSystems;
let inYear = document.getElementById("gameYear");
inYear.value = gameYear;
let inPrice = document.getElementById("gamePrice");
inPrice.value = gamePrice;
let inRating = document.getElementById("gameRating");
inRating.value = gameRating;

document.addEventListener("DOMContentLoaded", () => {
    const editForm = document.querySelector("#editForm");

    editForm.addEventListener("submit", e => {
        e.preventDefault();

        var name = editForm.gameName.value;
        var description = editForm.gameDescription.value;
        var developer = editForm.gameDeveloper.value;
        var publisher = editForm.gamePublisher.value;
        var genres = editForm.gameGenres.value;
        var systems = editForm.gameSystems.value;
        var year = editForm.gameYear.value;
        var price = editForm.gamePrice.value;
        var rating = editForm.gameRating.value;

        var isImg = editForm.gameImg.value;
        if(isImg != "")
        {
            console.log("Image is selected!");

            var img = editForm.gameImg.files[0];
            console.log(img);
            
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
                        replaceGame(imgPath,name,description,developer,publisher,genres,systems,year,price,rating);
                        alert("Game edited successfully!");
                    })
                    window.location.replace("./gameDetailedAdmin.html");
                    }
                else
                {
                    alert("Error while saving image!");
                }    
                })
        }
        else
        {
            let imgPath = gameImg;
            replaceGame(imgPath,name,description,developer,publisher,genres,systems,year,price,rating);
            alert("Game edited successfully!");
            window.location.replace("./gameDetailedAdmin.html");
        }
             
    })
})

function replaceGame(img,nam,desc,dev,pub,gen,sys,yr,pr,rt)
{
    fetch('https://localhost:7067/Game/ReplaceGame',
        {
            method: "POST",
            body: JSON.stringify({image:img,name:nam,description:desc,developer:dev,publisher:pub,genres:gen,systems:sys,year:yr,price:pr,rating:rt}),
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(s => {
            if(s.ok){
                console.log("Game edited successfully!");
            }
            else
            {
                alert("Error while editing game!");
            }
        })
}