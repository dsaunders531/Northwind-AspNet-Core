/* Detect which navbar element is active 
 * 
 * All navbar element ids need to follow a naming convention 'nav_controllername_actionname'.
 * The top level of a dropdown needs to be called 'nav_controllername_index' so it is made active when a submenu item is selected.
 * There must be one 'nav_home_index' which is the default if the id cannot be found.
 * 
 * The navbar id to make active is derived from the route (url). If you are using custom routes, you must change the nav 
 * element id so the controller name and action name is the same as the route.
 * 
 * eg: www.example.com/Home/Contact = 'nav_home_contact'
 *      www.example.com/Home = 'nav_home_index'
 *      www.example.com = 'nav_home_index'
 *      www.example.com/Products/Page/2 = 'nav_products_page'
 *      www.example.com/Products/?Page=2 = 'nav_products_index'
 * 
 * If the element id cannot be found, it is changed to 'nav_controllername_index' and if this cannot be found 'nav_home_index' is made active as the default.
 */

$(document).ready(function () { nav_ConfigureEvents(); });

/* Set the events to respond to.
 * 
 * The active nav entry needs to be set on resize. 
 * When moving between bootstrap screen sizes, the nav style changes so the active entry may need to be reset.
 * 
 * Force the navbar to remain on the top of the page after a scroll. 
 */
function nav_ConfigureEvents() {
    document.addEventListener("scroll", function () { nav_StayOnTop(); });
    window.addEventListener("resize", function () { nav_DetectActive(); });

    nav_DetectActive();
}


function nav_StayOnTop() {
    // to prevent crazy loop, the size of the window must be taken into account.
    if (window.innerWidth >= 768) {
        var eleHeader = $("header")[0];
        var eleNav = $("nav")[0];

        var pxHeaderHeight = eleHeader.getBoundingClientRect().height;

        if (window.scrollY > pxHeaderHeight) {
            var pxNavHeight = eleNav.getBoundingClientRect().height + 18;

            $("header").attr("style", "margin-top: " + pxNavHeight + "px; visibility: hidden;");
            $("nav").addClass("navbar-fixed-top");
        }
        else {
            if ($("nav").hasClass("navbar-fixed-top")) {
                $("header").attr("style", "");
                $("nav").removeClass("navbar-fixed-top");
            }
        }
    }
}

// Set the active class and screen reader text.
function nav_DetectActive() {
    var strRoute = window.location.pathname;
    strRoute = strRoute.replace(/#/i, "");

    // Find the controller and action from the url parts (assuming the pattern is: scheme host controller action)    
    var strsRoutePart = strRoute.split("/");
    var strControllerName = "home";
    var strActionName = "index";

    if (strsRoutePart.length >= 2) {
        strControllerName = strsRoutePart[1];
    }

    if (strsRoutePart.length >= 3) {
        strActionName = strsRoutePart[2];        
    }

    if (strControllerName == undefined) {
        strControllerName = "home";
    }
    else if(strControllerName.length == 0) {
        strControllerName = "home";
    }

    if (strActionName == undefined) {
        strActionName = "index";
    }
    else if (strActionName.length == 0) {
        strActionName = "index";
    }

    var strActiveNavId = this.BuildNavId(strControllerName, strActionName);

    this.nav_SetActive(strActiveNavId);
}

// Set the active state on the specified element id.
function nav_SetActive(strNavActiveEleId) {
    strNavActiveEleId = strNavActiveEleId.toLowerCase();
    
    var strsNavActiveEleId = strNavActiveEleId.split("_");

    // Find the parent id incase a child item is selected. 
    // When there are dropdown menus and the screen is desktop or greater, the top item needs to be made active.
    var strNavParentId = this.BuildNavId(strsNavActiveEleId[1], "index");

    // Remove the current active
    $("#navbar li.active a:has(span)").remove(".sr-only");
    $("#navbar li.active").removeClass("active");

    // Need to strip out leading #. jQuery needs it but standard JS does not.
    if (strNavActiveEleId.charAt(0) == "#") {
        strNavActiveEleId.replace(/#/i, "");
    }

    // Larger screens with horizontal menu
    if (document.getElementById(strNavParentId) != null) {
        if (window.innerWidth >= 768) {
            // Select the parent item on larger screens (Horizontal menu).
            if ($("#" + strNavParentId).hasClass("dropdown")) {
                strNavActiveEleId = strNavParentId;
            }
        }
        else {
            // smaller screens with vertical menu
            // set the item as active and open all its parent.
            if ($("#" + strNavParentId).hasClass("dropdown")) {
                $("#" + strNavParentId).addClass("open");
            }
        }
    }
    
    // See if the target element exists
    if (document.getElementById(strNavActiveEleId) == null) {       
        strNavActiveEleId = this.BuildNavId(strsNavActiveEleId[1], "index");

        if (document.getElementById(strNavActiveEleId) == null ) {
            // fallback value
            strNavActiveEleId = this.BuildNavId("home", "index");
        }
    }

    if (document.getElementById(strNavActiveEleId) != null) {
        $("#" + strNavActiveEleId).addClass("active");
        $("#" + strNavActiveEleId + " li.active a").add("<span class='sr-only'> (current)</span>");
    }
}

function BuildNavId(controllerName, actionName) {
    var strResult = "nav_" + controllerName + "_" + actionName;
    strResult = strResult.toLowerCase();
    return strResult;
}