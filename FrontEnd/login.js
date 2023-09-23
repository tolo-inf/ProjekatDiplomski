
function setFormMessage(formElement, type, message) {
    const messageElement = formElement.querySelector(".form__message");

    messageElement.textContent = message;
    messageElement.classList.remove("form__message--success", "form__message--error");
    messageElement.classList.add(`form__message--${type}`);
}
document.addEventListener("DOMContentLoaded", () => {
    const loginFormAdmin = document.querySelector("#loginAdmin");

    loginFormAdmin.addEventListener("submit", e => {
        e.preventDefault();
        
        var username = loginFormAdmin.username.value;
        var password = loginFormAdmin.password.value;
        
                     
        console.log(username);
        console.log(password);
        
        fetch('https://localhost:7067/Admin/LogIn',
        {
            method: "POST",
            body: JSON.stringify({username:username,password:password}),
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(s => {
            if(s.ok)
            {
                console.log("Log in successful!");
                s.json().then(k=>{

                    console.log(k.username);
                  
                    setFormMessage(loginFormAdmin, "success", "Success");

                    sessionStorage.setItem("usernameAdmin",k.username);
                    sessionStorage.setItem("passwordAdmin",k.password);
                    window.location.replace("./homepageAdmin.html");

                })
            }
            else{
                setFormMessage(loginFormAdmin, "error", "Invalid username/password combination");
            }
        })

 });
});