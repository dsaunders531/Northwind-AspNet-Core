/*
 * Responsive UI for categories.
 */
var viewModel = null;

var CategoryModel = function (categories) {
    var self = this;    
    self.categories = ko.observableArray(categories);   

    // Pagination items
    this.itemsPerPage = 5;
    this.currentPage = ko.observable(1);
    this.pages = ko.computed(function () {
        var result = Math.round(self.categories().length / self.itemsPerPage);

        if (result == 0) {
            result = 1;
        }
        return result;
    });    

    this.pageData = ko.computed(function () {
        var firstItem = (self.currentPage()-1) * self.itemsPerPage;
        return self.categories.slice(firstItem, firstItem + self.itemsPerPage);
    });

    this.hasPrevious = ko.computed(function () { return self.currentPage() > 1;});
    this.hasNext = ko.computed(function () { return self.currentPage() < self.pages(); });

    this.moveToFirst = function () {
        self.currentPage(1);
    };

    this.next = function () {
        if (self.currentPage() < self.pages()) {
            self.currentPage(self.currentPage() + 1);
        }
    };

    this.previous = function () {
        if (self.currentPage() > 1) {
            self.currentPage(self.currentPage() - 1);
        }
    };
    // end Pagination items
};

// When the page is loaded, get the data.
$(document).ready(function ()
{
    get();
});

function get() {
    try {
        $("#table").hide();
        $("#error").hide();
        $("#pagination").hide();
        $("#wait").show("slow");

        $.ajax({
            url: '/api/category/',
            type: 'GET',
            contentType: 'application/json; charset=UTF-8',
            success: function (data, textStatus, jqXHR) { get_result(jqXHR, textStatus, data); },
            error: function (jqXHR, textStatus, errorThrown) { get_error(jqXHR, textStatus, errorThrown); }
        });
    } catch (e) {
        get_error(null, "Get request could not be sent");
    }
}

function get_result(jqXHR, textStatus, data) {    
    try {
        $("#wait").hide();
        $("#error").hide();
        $("#table").show();
        $("#pagination").show();

        if (viewModel == null) {
            viewModel = new CategoryModel(data);
            ko.applyBindings(viewModel);
        }
        else {
            viewModel.currentPage(1);
            viewModel.categories.removeAll();
            for (var i = 0; i < data.length; i++) {
                viewModel.categories.push(data[i]);
            }
        }        
    } catch (e) {
        get_error(e);
    }
}

function get_error(jqXHR, textStatus, data) {
    $("#table").hide();
    $("#wait").hide();
    $("#pagination").hide();
    $("#error").show("slow");

    document.getElementById("error_message").innerText = textStatus + " " + JSON.stringify(data) + " " + jqXHR.responseText;    
}

// Edit (dialog) with save delete reset close, add new (dialog) with close.
// include validation.

var intSearchInterval = -1;

function search_keyDown() {
    try {
        window.clearTimeout(intSearchInterval);
    } catch (e) {
        alert(e);
    }    
}

function search_keyUp() {
    try {
        intSearchInterval = window.setTimeout(
                function () { search_start(); }, 750);
    } catch (e) {
        alert(e);
    }    
}

function search_start() {
    // Run the search. Put the results in the table.
    // The results are in the same format as the original get for all records so the 
    // same sucess and error pages are being used.
   
    try {
        search_keyDown(); // clear the interval.

        var strSearchterm = document.getElementById("searchbar").value;

        if (strSearchterm.length <= 0) {
            // get everything again;
            get();
        }
        else if (strSearchterm.length > 2) {
            $("#table").hide();
            $("#error").hide();
            $("#pagination").hide();
            $("#wait").show("slow");

            $.ajax({
                url: '/api/category/Search/?searchTerm=' + strSearchterm,
                type: 'GET',
                contentType: 'application/json; charset=UTF-8',
                success: function (data, textStatus, jqXHR) { get_result(jqXHR, textStatus, data); },
                error: function (jqXHR, textStatus, errorThrown) { get_error(jqXHR, textStatus, errorThrown); }
            });
        }
    } catch (e) {
        get_error(null, "Search request could not be sent");
    }
}