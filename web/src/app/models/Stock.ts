export class Stock {
    public symbol: string;
    public ask: number;
    public bid: number;

    constructor(symbol: string, ask: number, bid: number) {
        this.symbol = symbol;
        this.ask = ask;
        this.bid = bid;        
    }
}