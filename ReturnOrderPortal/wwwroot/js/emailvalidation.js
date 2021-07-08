function ValidateEmail() {
    var form = document.getElementById("form");
    var email = document.getElementById("email").value;
    var text = document.getElementById("text");
    var pattern = /^[^ ]+@[^ ]+\.[a-z]{2,3}$/;
    if (email.match(pattern)) {
        form.classList.add("Valid");
        form.classList.remove("Invalid");
        text.innerHTML = "Email is valid";
        text.style.color = "#00A300";

    }
    else {
        form.classList.remove("Valid");
        form.classList.add("Invalid");
        text.innerHTML = "Please enter valid Email address";
        text.style.color = "#ff0000";

    }
    if (emai = "") {
        form.classList.remove("Valid");
        form.classList.remove("Invalid");
        text.innerHTML = "";
        text.style.color = "#00ff00";
    }
    }
    
