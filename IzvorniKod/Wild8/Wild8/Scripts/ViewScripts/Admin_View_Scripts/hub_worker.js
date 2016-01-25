$(document).ready(function() {
    var ls = localStorage['orders'];
    if(ls == undefined) {
        localStorage['orders'] = new Set();
    }
    setUpHub();    
});

function setUpHub() {
    var hub = $.connection.OrderHub;
    var con = $.connection.hub;

    con.start().done(function() {
        
        hub.client.populateOrderStorage = function(orders, count) { saveOrders(orders); };

        hub.server.joinWorkerGroup();   //Join worker group

        hub.client.addNewOrder = function(order) { newOrderAdded(order) };

        hub.client.orderProcessed = function (order) { orderProcessed(order, hub) };

        $("#logout").one('click', function() {
            hub.server.leaveWorkerGroup();
        });
    });
}

//When order is added it is passed to all of active users 
//
function newOrderAdded(order) {
    //Save order to localStorage
    storeOrder(order);

    //Increment order count badge
    var oldOrderCount = parseInt($('#orders-badge').text());
    $("#orders-badge").html(oldOrderCount + 1);

    //If user is in orders page call controler to return a view for single order 
    //And append it to orders panel
    var ordersMenu = $(document).find("#orders-menu");
    if (ordersMenu != undefined) {
        var url = $('orders-menu').data('single-order-url');
        $.ajax({
            type: 'GET',
            url: url,
            data: order,
            success: function (orderPartial) {
                //Append order to order list 
                var ordersList = ordersMenu.find("#orders-list");
                $(orderPartial).css('display', 'none').appendTo(ordersList).slideDown('slow');
            },
            error: function(xhr, status, errorMsg) {
                
            }
        });
    }
}

//This function is used to allow multiple user to use system at the same time.
//When one user processes the order than all other users are notified and need 
//To remove that order from the list of orders.
//Process happens like this:
//  1.) user orders 
//  2.) notification of order comes to all of the users
//  3.) when one of the the user processes the order that notification goes to all other active users
//  4.) users removes the order from their session 
function orderProcessed(order, hub) {

}

//Prints all orders to the user 
function saveOrders(orders) {
    var jsOrdersObject = JSON.parse(orders);
    localStorage.setItem('orders', jsOrdersObject);
    $('#orders-badge').html(Object.keys(jsOrdersObject).length);
}

//Stores order in order array
function storeOrder(order) {
    var orders = localStorage.getItem('orders');
    var jsOrderObject = JSON.parse(order);
    orders.push(jsOrderObject);
    localStorage.setItem('orders', orders);
}

//Structures for checking if two orders are equal
function Order(order) {
    this.Name;
    this.Address;
    this.Phone;
    this.Email;
    this.TotalPrice;
    this.UserNote;
    this.OrderTime;
    this.Meals;
    this.equals = function(other) {
        if(Name != other.Name) return false;
        if(Address != other.Address) return false;
        if(Phone != other.Phone) return false;
        if(Email != other.Email) return false;
        if(TotalPrice != other.TotalPrice) return false;
        if(UserNote != other.UserNote) return false;
        if(OrderTime != other.OrderTime) return false;
        
        if(Meals.length != other.Meals.length) return false;
        for(var i = 0; i < Meals.length; i++) {
            if(!Meals[i].equals(other.Meals[i])) return false;   
        }
        
        return true;
    }
    
    //Take all of th properties parsed from JSON
    for(var prop in order) {
        if(prop == 'Meals') {
            var meals = [];
            for(var i = 0; i < prop.length; i++) {
                meals[i] = new Meal(order[prop][i]);
            }   
        } else {
            this[prop] = order[prop];
        }
    }
    
}

function Meal(meal) {
     this.MealName;
     this.MealTypeName; 
     this.Count;
     this.Addons;
     this.equals = function(other) {
        if(MealName != other.MealName) return false;
        if(MealTypeName != other.MealTypeName) return false;
        if(Count != other.Count) return false;
        if(Addons.length != other.Addons.length) return false;
        
        for(var i = 0; i < Addons.length; i++) {
            if(!Addons[i].equals(other.Addons[i])) return false;
        }
        
        return true;         
     }
     
     for(var prop in meal) {
         if(prop != 'Addons') {
             this[prop] = meal[prop];
         } else {
             this[prop] = new AddOns(meal[prop]);
         }        
     }
}

function AddOns(Addons) {
    this.Addons = Addons;
    this.equals = function(other) {
        if(Addons.length != other.addons.length) return false;
        
        for(var i = 0; i < Addons.length; i++) {
            if(Addons[i] != other.addons[i]) return false;
        }
        return true;
   };
}


function getOrderPartialView() {
    return null;
}
    
    
    
    
    
    
    
    
    
}