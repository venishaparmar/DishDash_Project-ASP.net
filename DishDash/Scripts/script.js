async function loadDatabase() {
  const db = await idb.openDB("dishDash", 1, {
    upgrade(db, oldVersion, newVersion, transaction) {
      db.createObjectStore("products", {
        keyPath: "id",
        autoIncrement: true,
      });
      db.createObjectStore("sales", {
        keyPath: "id",
        autoIncrement: true,
      });
    },
  });

  return {
    db,
    getProducts: async () => await db.getAll("products"),
    addProduct: async (product) => await db.add("products", product),
    editProduct: async (product) =>
      await db.put("products", product.id, product),
    deleteProduct: async (product) => await db.delete("products", product.id),
  };
}



function initApp() {
  const app = {
    db: null,
    time: null,
    firstTime: localStorage.getItem("first_time") === null,
    activeMenu: 'pos',
    loadingSampleData: false,
    moneys: [10, 50, 100, 200, 500, 2000],   
    categories: [
      { id: 1, name: "Burger", icon: "img/beef-burger.png" },
      { id: 2, name: "Pizza", icon: "img/sandwich.png" },
      { id: 3, name: "Category 3", icon: "img/cinnamon-roll.png" },
      // Add IDs to other categories as needed
    ],

    
    products: [
      {
        "id": 1,
        "name": "Beef Burger",
        "price": 45000,
        "image": "img/beef-burger.png",
        "options": null,
        "categoryId": 1
      },
      {
        "id": 2,
        "name": "Sandwich",
        "price": 32000,
        "image": "img/sandwich.png",
        "options": null,
        "categoryId": 2
      },
      {
        "id": 3,
        "name": "Sawarma",
        "price": 30000,
        "image": "img/sawarma.png",
        "options": null,
        "categoryId": 3
      },
      {
        "id": 4,
        "name": "Croissant",
        "price": 16000,
        "image": "img/croissant.png",
        "options": null,
        "categoryId": 1
      },
      {
        "id": 5,
        "name": "Cinnamon Roll",
        "price": 20000,
        "image": "img/cinnamon-roll.png",
        "options": null,
        "categoryId": 2
      },
      {
        "id": 6,
        "name": "Choco Glaze Donut with Peanut",
        "price": 10000,
        "image": "img/choco-glaze-donut-peanut.png",
        "options": null,
        "categoryId": 3
      },
      {
        "id": 7,
        "name": "Choco Glazed Donut",
        "price": 10000,
        "image": "img/choco-glaze-donut.png",
        "options": null,
        "categoryId": 2
      },
      {
        "id": 8,
        "name": "Red Glazed Donut",
        "price": 10000,
        "image": "img/red-glaze-donut.png",
        "options": null,
        "categoryId": 1
      },
      {
        "id": 9,
        "name": "Iced Coffee Latte",
        "price": 25000,
        "image": "img/coffee-latte.png",
        "options": null,
        "categoryId": 3
      },
      {
        "id": 10,
        "name": "Iced Chocolate",
        "price": 20000,
        "image": "img/ice-chocolate.png",
        "options": null,
        "categoryId": 3
      },
      {
        "id": 11,
        "name": "Iced Tea",
        "price": 15000,
        "image": "img/ice-tea.png",
        "options": null,
        "categoryId": 1
      },
      {
        "id": 12,
        "name": "Iced Matcha Latte",
        "price": 22000,
        "image": "img/matcha-latte.png",
        "options": null,
        "categoryId": 2
      }
    ],
    keyword: "",
    cart: [],
    cash: 0,
    change: 0,
    isShowModalReceipt: false,
    receiptNo: null,
    receiptDate: null,
    async initDatabase() {
    },
    filteredProducts() {
      const rg = this.keyword ? new RegExp(this.keyword, "gi") : null;
      return this.products.filter((p) => {
        return ((!rg || p.name.match(rg)) &&
                (!this.selectedCategory || p.categoryId === this.selectedCategory));
      });
    },

    showAllProducts() {
      this.selectedCategory = null; 
    },

    filterByCategory(categoryId) {
      this.selectedCategory = categoryId === "all" ? null : categoryId;
    },
    
    addToCart(product) {
      const index = this.findCartIndex(product);
      if (index === -1) {
        this.cart.push({
          productId: product.id,
          image: product.image,
          name: product.name,
          price: product.price,
          option: product.option,
          qty: 1,
        });
      } else {
        this.cart[index].qty += 1;
      }
      this.beep();
      this.updateChange();
    },
    findCartIndex(product) {
      return this.cart.findIndex((p) => p.productId === product.id);
    },
    addQty(item, qty) {
      const index = this.cart.findIndex((i) => i.productId === item.productId);
      if (index === -1) {
        return;
      }
      const afterAdd = item.qty + qty;
      if (afterAdd === 0) {
        this.cart.splice(index, 1);
        this.clearSound();
      } else {
        this.cart[index].qty = afterAdd;
        this.beep();
      }
      this.updateChange();
    },
    getItemsCount() {
      return this.cart.reduce((count, item) => count + item.qty, 0);
    },
    updateChange() {
      this.change = this.cash - this.getTotalPrice();
    },
    updateCash(value) {
      this.cash = parseFloat(value.replace(/[^0-9]+/g, ""));
      this.updateChange();
    },
    getTotalPrice() {
      return this.cart.reduce(
        (total, item) => total + item.qty * item.price,
        0
      );
    },
    submit() {
      const time = new Date();
      this.isShowModalReceipt = true;
      this.receiptNo = `Dish-Dash-${Math.round(time.getTime() / 1000)}`;
      this.receiptDate = this.dateFormat(time);
    },
    closeModalReceipt() {
      this.isShowModalReceipt = false;
    },
    dateFormat(date) {
      const formatter = new Intl.DateTimeFormat('id', { dateStyle: 'short', timeStyle: 'short'});
      return formatter.format(date);
    },
    numberFormat(number) {
      return (number || "")
        .toString()
        .replace(/^0|\./g, "")
        .replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
    },
    priceFormat(number) {
      return number ? `Rs. ${this.numberFormat(number)}` : `Rs. 0`;
    },
    clear() {
      this.cash = 0;
      this.cart = [];
      this.receiptNo = null;
      this.receiptDate = null;
      this.updateChange();
      this.clearSound();
    },
    beep() {
      this.playSound("sound/beep-29.mp3");
    },
    clearSound() {
      this.playSound("sound/button-21.mp3");
    },
    playSound(src) {
      const sound = new Audio();
      sound.src = src;
      sound.play();
      sound.onended = () => delete(sound);
    },
    printAndProceed() {
      const receiptContent = document.getElementById('receipt-content');
      const titleBefore = document.title;
      const printArea = document.getElementById('print-area');

      printArea.innerHTML = receiptContent.innerHTML;
      document.title = this.receiptNo;

      window.print();
      this.isShowModalReceipt = false;

      printArea.innerHTML = '';
      document.title = titleBefore;

      // TODO save sale data to database

      this.clear();
    },
    
    
  };

  return app;
}