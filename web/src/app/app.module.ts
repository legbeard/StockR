import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StockTableComponent } from './stock-table/stock-table.component';
import { ApiInterceptor } from './services/api.interceptor';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';

@NgModule({
  declarations: [
    AppComponent,
    StockTableComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    MatTableModule,
    MatIconModule,
    MatToolbarModule
  ],
  providers: [ { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
