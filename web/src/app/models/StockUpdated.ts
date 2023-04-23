import { Stock } from "./Stock";

export class StockUpdated {
    stock: Stock;
    timestamp: Date;

    constructor(stock: Stock, timestamp: Date) {
        this.stock = stock;
        this.timestamp = timestamp;
    }
}