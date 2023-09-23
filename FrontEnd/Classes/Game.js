export class Game{
    constructor(id, img, name, description, developer, publisher, genres, systems, year, price, rating)
    {
        this.id = id;
        this.img = img
        this.name = name;
        this.description = description;
        this.developer = developer;
        this.publisher = publisher;
        this.genres = genres;
        this.systems = systems;
        this.year = year;
        this.price = price;
        this.rating = rating;
        this.container = null;
    }

    drawGame(host)
    {
        let tr = document.createElement("tr");
        host.appendChild(tr);

        let el = document.createElement("td");
        el.innerHTML="<img src='../wwwroot/" + this.img + "'" + "alt='gameImage' style='width:320px;height:140px;'/>";
        el.onclick=(event)=>this.viewDetails();
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML=this.name;
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML=this.developer;
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML=this.publisher;
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML=this.genres;
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML=this.systems;
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML=this.year;
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML=this.price + "$";
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML=this.rating;
        tr.appendChild(el);
    }

    viewDetails()
    {
        localStorage.setItem("gameId",this.id);
        localStorage.setItem("gameImg",this.img);
        localStorage.setItem("gameName",this.name);
        localStorage.setItem("gameDescription",this.description);
        localStorage.setItem("gamePublisher",this.publisher);
        localStorage.setItem("gameDeveloper",this.developer);
        localStorage.setItem("gameGenres",this.genres);
        localStorage.setItem("gameSystems",this.systems);
        localStorage.setItem("gameYear",this.year);
        localStorage.setItem("gamePrice",this.price);
        localStorage.setItem("gameRating",this.rating);
        let adminUsername = sessionStorage.getItem("usernameAdmin");
        if(adminUsername != null)
        {
            window.location.href = "gameDetailedAdmin.html"
        }
        else
        {
            window.location.href = "gameDetailed.html";
        }
        
    }

    fillDetails()
    {
        let imgGame = document.querySelector(".imgGame");
        imgGame.src = "../wwwroot/" + this.img;

        let lblGameName = document.querySelector(".lblGameName");
        lblGameName.innerHTML = this.name;

        let lblGameDescription = document.querySelector(".lblGameDescription");
        lblGameDescription.innerHTML = this.description;

        let lblGameDeveloper = document.querySelector(".lblGameDeveloper");
        lblGameDeveloper.innerHTML = this.developer;

        let lblGamePublisher = document.querySelector(".lblGamePublisher");
        lblGamePublisher.innerHTML = this.publisher;

        let lblGameGenres = document.querySelector(".lblGameGenres");
        lblGameGenres.innerHTML = this.genres;

        let lblGameSystems = document.querySelector(".lblGameSystems");
        lblGameSystems.innerHTML = this.systems;

        let lblGameYear = document.querySelector(".lblGameYear");
        lblGameYear.innerHTML = this.year;

        let lblGamePrice = document.querySelector(".lblGamePrice");
        lblGamePrice.innerHTML = this.price + "$";

        let lblGameRating = document.querySelector(".lblGameRating");
        lblGameRating.innerHTML = this.rating;
    }
}