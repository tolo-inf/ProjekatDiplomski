import { Game } from "./Classes/Game.js";

function setFormMessage(formElement, type, message) {
    const messageElement = formElement.querySelector(".form__message");
  
    messageElement.textContent = message;
    messageElement.classList.remove("form__message--success", "form__message--error");
    messageElement.classList.add(`form__message--${type}`);
}

let adminUsername = sessionStorage.getItem("usernameAdmin");
let anonymous = localStorage.getItem("anonymous");

if(adminUsername == null)
{
    document.body.style.visibility = "hidden";
}
if(anonymous == "yes")
{
    document.body.style.visibility = "visible";
}


document.addEventListener("DOMContentLoaded", () => {
    const searchForm = document.querySelector("#searchForm");

    searchForm.addEventListener("submit", e => {
        e.preventDefault();

        var name = searchForm.gameName.value;
        var description = searchForm.gameDescription.value;
        var developer = searchForm.gameDeveloper.value;
        var publisher = searchForm.gamePublisher.value;
        var genres = searchForm.gameGenres.value;
        var systems = searchForm.gameSystems.value;
        var yearStart = searchForm.gameYearFrom.value;
        var yearEnd = searchForm.gameYearTo.value;
        var priceStart = searchForm.gamePriceFrom.value;
        var priceEnd = searchForm.gamePriceTo.value;
        var ratingStart = searchForm.gameRatingFrom.value;
        var ratingEnd = searchForm.gameRatingTo.value;

        if(yearStart == null || yearStart == "")
            yearStart = 0;
        if(yearEnd == null || yearEnd == "")
            yearEnd = 0;
        if(priceStart == null || priceStart == "")
            priceStart = 0;
        if(priceEnd == null || priceEnd == "")
            priceEnd = 0;
        if(ratingStart == null || ratingStart == "")
            ratingStart = 0;
        if(ratingEnd == null || ratingEnd == "")
            ratingEnd = 0;

        fetch('https://localhost:7067/Game/PerformSearch',
        {
            method: "POST",
            body: JSON.stringify({name:name,description:description,developer:developer,publisher:publisher,genres:genres,systems:systems,yearStart:yearStart,yearEnd:yearEnd,priceStart:priceStart,priceEnd:priceEnd,ratingStart:ratingStart,ratingEnd:ratingEnd}),
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(s => {
            if(s.ok){
                console.log("Search performed successfully!");
                deletePreviousTable();
                drawTable(document.body);
                s.json().then(data => {
                    data.forEach(g => {

                        let game = new Game(g.id, g.image, g.name, g.description, g.developer, g.publisher, g.genres, g.systems, g.year, g.price, g.rating);

                        let divTable = document.querySelector(".tableBody");
                        game.drawGame(divTable);
                    })
                })
            }
            else
            {
                alert("Error while performing searching!");
            }
        })
        
    })
})

function drawTable(host)
{
    let tableFrame = document.createElement("table");
    tableFrame.classList.add("tableFrame");
    host.appendChild(tableFrame);

    let tableHead= document.createElement("thead");
    tableFrame.appendChild(tableHead);

    let tr = document.createElement("tr");
    tableHead.appendChild(tr);

    let tableBody = document.createElement("tbody");
    tableBody.classList.add("tableBody");
    tableBody.id = "tableBody";
    tableFrame.appendChild(tableBody);

    let th;
    let zag=["Image", "Name", "Developer", "Publisher", "Genres", "Systems", "Release Year", "Price", "Rating"];
    zag.forEach(el=>{
        th = document.createElement("th");
        th.innerHTML=el;
        tr.appendChild(th);
    })
}

function deletePreviousTable()
    {
        let tableBody = document.querySelector(".tableBody");

        if(tableBody == null)
            return;
        
        let tableFrame = tableBody.parentNode;
        tableFrame.removeChild(tableBody);

        let parent = tableFrame.parentNode;
        parent.removeChild(tableFrame);
    }