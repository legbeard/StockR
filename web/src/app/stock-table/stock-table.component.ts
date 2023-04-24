import { Component } from '@angular/core';
import { Stock } from '../models/Stock';
import { StockService } from '../services/stock.service';
import { StockUpdated } from '../models/StockUpdated';

@Component({
  selector: 'app-stock-table',
  templateUrl: './stock-table.component.html',
  styleUrls: ['./stock-table.component.css']
})

export class StockTableComponent {

  constructor(private stockService: StockService) {}

  stocks: Stock[] = [];
  displayedColumns: string[] = ['symbol', 'bid', 'bid-change', 'ask', 'ask-change']

  ngOnInit(){
    this.stocks = []
    this.stockService.getStocks().subscribe(stocks => {
      this.stocks = stocks;
    })
    this.stockService.getUpdateStream().subscribe(stockUpdate => this.updateStock(stockUpdate));
  }

  updateStock(update: Stock) : void {
    let stock = this.stocks.find(stock => stock.symbol == update.symbol);
    if(stock){
      stock.ask = update.ask;
      stock.bid = update.bid;
      stock.askChange = update.askChange;
      stock.bidChange = update.bidChange;
    }
  }
}
