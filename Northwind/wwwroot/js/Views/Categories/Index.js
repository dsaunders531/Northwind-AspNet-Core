/*
 * Responsive UI for categories.
 */

var CategoryModel = function (categories) {
    var self = this;    
    self.categories = ko.observableArray(categories);   

    // Pagination items
    this.itemsPerPage = 3;
    this.currentPage = ko.observable(1);
    this.pages = ko.computed(function () { return Math.round(self.categories().length / self.itemsPerPage); });    

    this.pageData = ko.computed(function () {
        var firstItem = (self.currentPage()-1) * self.itemsPerPage;
        return self.categories.slice(firstItem, firstItem + self.itemsPerPage);
    });

    this.hasPrevious = ko.computed(function () { return self.currentPage() > 1;});
    this.hasNext = ko.computed(function () { return self.currentPage() < self.pages(); });

    this.next = function () {
        if (self.currentPage() < self.pages()) {
            self.currentPage( self.currentPage() + 1);
        }
    };
    this.previous = function () {
        if (self.currentPage() > 1) {
            self.currentPage( self.currentPage() - 1);
        }
    };
    // end Pagination items
};

// When the page is loaded, get the data.
$(document).ready(function ()
{
    $("#table").hide("fast");
    $("#error").hide("fast");
    $("#wait").show("slow");

    try {
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
});

function get_result(jqXHR, textStatus, data) {    
    $("#wait").hide("fast");
    $("#error").hide("slow");
    $("#table").show("slow");

    var viewModel = new CategoryModel(data);
    ko.applyBindings(viewModel);
}

function get_error(jqXHR, textStatus, data) {
    $("#table").hide("fast");
    $("#wait").hide("fast");
    $("#error").show("slow");

    document.getElementById("error_message").innerText = textStatus + " " + JSON.stringify(data) + " " + jqXHR.responseText;    
}

// TODO search (key up, key down), edit (dialog) with save delete reset close, add new (dialog) with close.
// include validation.