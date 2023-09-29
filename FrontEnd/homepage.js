import { Game } from "./Classes/Game.js";

function setFormMessage(formElement, type, message) {
    const messageElement = formElement.querySelector(".form__message");
  
    messageElement.textContent = message;
    messageElement.classList.remove("form__message--success", "form__message--error");
    messageElement.classList.add(`form__message--${type}`);
}

document.addEventListener("DOMContentLoaded", () => {
    const searchForm = document.querySelector("#searchForm");

    searchForm.addEventListener("submit", e => {
        e.preventDefault();

        let countAND = 0;
        let countOR = 0;
        let countNOT = 0;

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
           { ratingEnd = 0;}

        let cbxName = "";
        console.log(cbxName === "");
        if(document.getElementById("cbxNameAND").checked)
        {
            cbxName = "AND";
            countAND++;
        }
        else if(document.getElementById("cbxNameOR").checked)
        {
            cbxName = "OR";
            countOR++;
        }
        else if(document.getElementById("cbxNameNOT").checked)
        {
            cbxName = "NOT";
            countNOT++;
        }

        let cbxDesc = "";
        if(document.getElementById("cbxDescAND").checked)
        {
            cbxDesc = "AND";
            countAND++;
        }
        else if(document.getElementById("cbxDescOR").checked)
        {
            cbxDesc = "OR";
            countOR++;
        }
        else if(document.getElementById("cbxDescNOT").checked)
        {
            cbxDesc = "NOT";
            countNOT++;
        }

        let cbxDev = "";
        if(document.getElementById("cbxDevAND").checked)
        {
            cbxDev = "AND";
            countAND++;
        }
        else if(document.getElementById("cbxDevOR").checked)
        {
            cbxDev = "OR";
            countOR++;
        }
        else if(document.getElementById("cbxDevNOT").checked)
        {
            cbxDev = "NOT";
            countNOT++;
        }

        let cbxPub = "";
        if(document.getElementById("cbxPubAND").checked)
        {
            cbxPub = "AND";
            countAND++;
        }
        else if(document.getElementById("cbxPubOR").checked)
        {
            cbxPub = "OR";
            countOR++;
        }
        else if(document.getElementById("cbxPubNOT").checked)
        {
            cbxPub = "NOT";
            countNOT++;
        }

        let cbxGen = "";
        if(document.getElementById("cbxGenAND").checked)
        {
            cbxGen = "AND";
            countAND++;
        }
        else if(document.getElementById("cbxGenOR").checked)
        {
            cbxGen = "OR";
            countOR++;
        }
        else if(document.getElementById("cbxGenNOT").checked)
        {
            cbxGen = "NOT";
            countNOT++;
        }

        let cbxSys = "";
        if(document.getElementById("cbxSysAND").checked)
        {
            cbxSys = "AND";
            countAND++;
        }
        else if(document.getElementById("cbxSysOR").checked)
        {
            cbxSys = "OR";
            countOR++;
        }
        else if(document.getElementById("cbxSysNOT").checked)
        {
            cbxSys = "NOT";
            countNOT++;
        }

        if(name.length == 0 && description.length == 0 && developer.length == 0 && publisher.length == 0 && genres.length == 0 && systems.length == 0)
        {
            cbxName = "AND";
            cbxDesc = "AND";
            cbxDev = "AND";
            cbxPub = "AND";
            cbxGen = "AND";
            cbxSys = "AND";
        }

        if(countAND != 0)
        {
            if(cbxName === "")
            {
                cbxName = "AND";
            }
            if(cbxDesc === "")
            {
                cbxDesc = "AND";
            }
            if(cbxDev === "")
            {
                cbxDev = "AND";
            }
            if(cbxPub === "")
            {
                cbxPub = "AND";
            }
            if(cbxGen === "")
            {
                cbxGen = "AND";
            }
            if(cbxSys === "")
            {
                cbxSys = "AND";
            }
        }
        else if(countOR != 0)
        {
            if(cbxName === "")
            {
                cbxName = "OR";
            }
            if(cbxDesc === "")
            {
                cbxDesc = "OR";
            }
            if(cbxDev === "")
            {
                cbxDev = "OR";
            }
            if(cbxPub === "")
            {
                cbxPub = "OR";
            }
            if(cbxGen === "")
            {
                cbxGen = "OR";
            }
            if(cbxSys === "")
            {
                cbxSys = "OR";
            }
        }
        else
        {
            if(cbxName === "")
            {
                cbxName = "AND";
            }
            if(cbxDesc === "")
            {
                cbxDesc = "AND";
            }
            if(cbxDev === "")
            {
                cbxDev = "AND";
            }
            if(cbxPub === "")
            {
                cbxPub = "AND";
            }
            if(cbxGen === "")
            {
                cbxGen = "AND";
            }
            if(cbxSys === "")
            {
                cbxSys = "AND";
            }
        }

        fetch('https://localhost:7067/Game/PerformSearch',
        {
            method: "POST",
            body: JSON.stringify({name:name,description:description,developer:developer,publisher:publisher,genres:genres,systems:systems,yearStart:yearStart,yearEnd:yearEnd,priceStart:priceStart,priceEnd:priceEnd,ratingStart:ratingStart,ratingEnd:ratingEnd,opName:cbxName,opDesc:cbxDesc,opDev:cbxDev,opPub:cbxPub,opGen:cbxGen,opSys:cbxSys}),
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

let btnSearchAnywhere = document.getElementById("btnSearchGamesAnywhere");
btnSearchAnywhere.addEventListener("click", searchAnywhere);

function searchAnywhere()
{
    let countAND = 0;
        let countOR = 0;
        let countNOT = 0;

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
           { ratingEnd = 0;}

        let cbxName = "";
        console.log(cbxName === "");
        if(document.getElementById("cbxNameAND").checked)
        {
            cbxName = "AND";
            countAND++;
        }
        else if(document.getElementById("cbxNameOR").checked)
        {
            cbxName = "OR";
            countOR++;
        }
        else if(document.getElementById("cbxNameNOT").checked)
        {
            cbxName = "NOT";
            countNOT++;
        }

        let cbxDesc = "";
        if(document.getElementById("cbxDescAND").checked)
        {
            cbxDesc = "AND";
            countAND++;
        }
        else if(document.getElementById("cbxDescOR").checked)
        {
            cbxDesc = "OR";
            countOR++;
        }
        else if(document.getElementById("cbxDescNOT").checked)
        {
            cbxDesc = "NOT";
            countNOT++;
        }

        let cbxDev = "";
        if(document.getElementById("cbxDevAND").checked)
        {
            cbxDev = "AND";
            countAND++;
        }
        else if(document.getElementById("cbxDevOR").checked)
        {
            cbxDev = "OR";
            countOR++;
        }
        else if(document.getElementById("cbxDevNOT").checked)
        {
            cbxDev = "NOT";
            countNOT++;
        }

        let cbxPub = "";
        if(document.getElementById("cbxPubAND").checked)
        {
            cbxPub = "AND";
            countAND++;
        }
        else if(document.getElementById("cbxPubOR").checked)
        {
            cbxPub = "OR";
            countOR++;
        }
        else if(document.getElementById("cbxPubNOT").checked)
        {
            cbxPub = "NOT";
            countNOT++;
        }

        let cbxGen = "";
        if(document.getElementById("cbxGenAND").checked)
        {
            cbxGen = "AND";
            countAND++;
        }
        else if(document.getElementById("cbxGenOR").checked)
        {
            cbxGen = "OR";
            countOR++;
        }
        else if(document.getElementById("cbxGenNOT").checked)
        {
            cbxGen = "NOT";
            countNOT++;
        }

        let cbxSys = "";
        if(document.getElementById("cbxSysAND").checked)
        {
            cbxSys = "AND";
            countAND++;
        }
        else if(document.getElementById("cbxSysOR").checked)
        {
            cbxSys = "OR";
            countOR++;
        }
        else if(document.getElementById("cbxSysNOT").checked)
        {
            cbxSys = "NOT";
            countNOT++;
        }

        if(name.length == 0 && description.length == 0 && developer.length == 0 && publisher.length == 0 && genres.length == 0 && systems.length == 0)
        {
            cbxName = "AND";
            cbxDesc = "AND";
            cbxDev = "AND";
            cbxPub = "AND";
            cbxGen = "AND";
            cbxSys = "AND";
        }

        if(countAND != 0)
        {
            if(cbxName === "")
            {
                cbxName = "AND";
            }
            if(cbxDesc === "")
            {
                cbxDesc = "AND";
            }
            if(cbxDev === "")
            {
                cbxDev = "AND";
            }
            if(cbxPub === "")
            {
                cbxPub = "AND";
            }
            if(cbxGen === "")
            {
                cbxGen = "AND";
            }
            if(cbxSys === "")
            {
                cbxSys = "AND";
            }
        }
        else if(countOR != 0)
        {
            if(cbxName === "")
            {
                cbxName = "OR";
            }
            if(cbxDesc === "")
            {
                cbxDesc = "OR";
            }
            if(cbxDev === "")
            {
                cbxDev = "OR";
            }
            if(cbxPub === "")
            {
                cbxPub = "OR";
            }
            if(cbxGen === "")
            {
                cbxGen = "OR";
            }
            if(cbxSys === "")
            {
                cbxSys = "OR";
            }
        }
        else
        {
            if(cbxName === "")
            {
                cbxName = "AND";
            }
            if(cbxDesc === "")
            {
                cbxDesc = "AND";
            }
            if(cbxDev === "")
            {
                cbxDev = "AND";
            }
            if(cbxPub === "")
            {
                cbxPub = "AND";
            }
            if(cbxGen === "")
            {
                cbxGen = "AND";
            }
            if(cbxSys === "")
            {
                cbxSys = "AND";
            }
        }

        fetch('https://localhost:7067/Game/PerformSearchAnywhere',
        {
            method: "POST",
            body: JSON.stringify({name:name,description:description,developer:developer,publisher:publisher,genres:genres,systems:systems,yearStart:yearStart,yearEnd:yearEnd,priceStart:priceStart,priceEnd:priceEnd,ratingStart:ratingStart,ratingEnd:ratingEnd,opName:cbxName,opDesc:cbxDesc,opDev:cbxDev,opPub:cbxPub,opGen:cbxGen,opSys:cbxSys}),
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
}