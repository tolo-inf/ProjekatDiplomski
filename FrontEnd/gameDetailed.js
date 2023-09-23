import { Game } from "./Classes/Game.js";

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
let game;
console.log(gameId);
console.log(gameName);
console.log(gameImg);

fetch('https://localhost:7067/Game/GetGameByName?name='+encodeURIComponent(gameName),{
    method:'GET'
  }).then( s => {
    if(s.ok)
    {
      s.json().then(data => {
        console.log(data);
        game = new Game(data.id, data.image, data.name, data.description, data.developer, data.publisher, data.genres, data.systems, data.year, data.price, data.rating);
        game.fillDetails();
      });
    }
    else
    {
      alert("Game with this name does not exist!");
    }
  });

let btnDeleteGame = document.getElementById("btnDeleteGame");
btnDeleteGame.addEventListener("click", deleteGame);

function deleteGame()
{
  var retVal = confirm("Do you want to continue?");
  if( retVal == true ) 
  {
    console.log("Delete approved!");
    fetch('https://localhost:7067/Game/DeleteGame?name='+encodeURIComponent(gameName),{
      method:'DELETE'
    }).then( s => {
      if(s.ok)
      {
        alert("Game successfully deleted!");
        window.location = "./homepageAdmin.html";
      }
      else
      {
        alert("Game with this name does not exist!");
      }
    });

  } 
  else 
  {
    console.log("Delete canceled!");
  }
}

let btnEditGame = document.getElementById("btnEditGame");
btnEditGame.addEventListener("click", editGame);

function editGame()
{
  window.location = "./editGame.html";
}