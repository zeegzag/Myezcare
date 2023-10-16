(function (url, title, text, webchatId) {

    var body = document.getElementsByTagName('body');
    var container = document.createElement('div');
    container.id = 'livechatcontainer';
    container.style.position = "absolute";
    container.style.right = "25px";
    container.style.bottom = "45px";
    container.style.maxHeight = "600px";
    body[0].appendChild(container);
    var iconContainer = document.createElement('div');
    iconContainer.style.position = "fixed";
    iconContainer.style.bottom = "50px";
    iconContainer.style.right = "20px";
    iconContainer.style.zIndex = "50001";
    var img = document.createElement('img');
    img.className = "livechatopenbutton";
    img.src = url + "/icon.png";
    img.style.cursor = "pointer";

    var div2 = document.createElement('div');

    div2.style.bottom = "25px"
    div2.style.right = "25px";
    div2.style.textAlign = "right";
    //div2.innerHTML = '<img class="livechatopenbutton" src="' + url + '/icon.png">';
    img.addEventListener('click', function () {

        var e = document.getElementsByClassName("livechatdialogcontent")[0];
        var style = window.getComputedStyle(e);
        console.log(style)
        if (style.display === "none") {
            e.style.display = "block";
            img.src = url + "/iconClose.png";
        }
        else {
            e.style.display = "none";
            img.src = url + "/icon.png";
        }

    });
    div2.appendChild(img);
    iconContainer.appendChild(div2);
    var welcomeDiv = document.createElement('div');
    welcomeDiv.className = "livechatdialogcontent";
    welcomeDiv.style.display = "none";
    welcomeDiv.innerHTML = '<div class="livechatwelcome"><h3>' + title + '</h3><p>' + text + '</p><input class="livechatinput" id="livechatname" name="name" placeholder="Your name" /><input id="livechatemail" class="livechatinput" type="email" name="email" placeholder="Contact email"/> <button id="livechatopenchat">Next</button></div>';



    container.appendChild(welcomeDiv);
    var openchat = document.getElementById("livechatopenchat");
    openchat.addEventListener('click', function () {
        welcomeDiv.innerHTML = '<div style="right: 25px;"><iframe src="' + url + '/?webChatId=' + webchatId + '&username=' + document.getElementById('livechatname').value + '&email=' + document.getElementById('livechatemail').value + '" width="300px"  style="height:80vh; max-height:600px" frameborder="0" /></div>'
    });

    container.appendChild(iconContainer);

    var link = document.createElement('link');
    link.setAttribute('rel', 'stylesheet');
    link.type = 'text/css';
    link.href = url + '/chat.css';
    document.head.appendChild(link);
})('https://frontend.chat.delegate.cloud', 'Customer Support', 'before we can begin we need to know a little about you', '27e59958-0e3a-449f-b424-4afea67ced68');