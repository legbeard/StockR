import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';
import { Stock } from '../models/Stock';
import { Observable, Subject } from 'rxjs';
import { StockUpdated } from '../models/StockUpdated';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StockService {

  private hubConnection: HubConnection;
  private stockUpdatedSubject : Subject<StockUpdated>

  constructor(private http: HttpClient) { 
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/Stock/Hub`, {
        // TODO: Don't do this in prod!
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .build();

    this.stockUpdatedSubject = new Subject<StockUpdated>();

    this.hubConnection.start()
    .then(_ => {
      this.hubConnection.on('SendStockUpdate', update => {
        this.stockUpdatedSubject.next(update);
      });
    });

  }

  getStocks() : Observable<Stock[]> {
    return this.http.get<Stock[]>('/stock');
  }

  getStock(symbol: string) : Observable<Stock> {
    return this.http.get<Stock>(`/stock/${symbol}`)
  }

  getUpdateStream(): Observable<StockUpdated>{
    return this.stockUpdatedSubject.asObservable();
  }
}
