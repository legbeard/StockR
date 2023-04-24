import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StockTableComponent } from './stock-table/stock-table.component';

const routes: Routes = [
  {  path: '', redirectTo: '/overview', pathMatch: 'full'},
  {  path: 'overview', component: StockTableComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
