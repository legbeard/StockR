export class StockUpdated {
    constructor(public symbol: string, public bid: number, public ask: number, public timestamp: Date) {    }
}