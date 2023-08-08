import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any = [];
  public filtredEvents: any = [];
  widthImg: number = 150;
  marginImg: number = 2;
  showImg: boolean = true;
  private _listFilter: string = '';

  public get listFilter(): string {
    return this._listFilter;
  }

  public set listFilter(value: string){
    this._listFilter = value;
    this.filtredEvents = this.listFilter ? this.eventFilter(this.listFilter) : this.eventos;
  }

  eventFilter(filter: string): any {
    filter = filter.toLowerCase();
    return this.eventos.filter(
      (evento: {tema: string; local: string;}) => evento.tema.toLocaleLowerCase().indexOf(filter) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filter) !== -1

    )
  }

  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getEventos()
  }

  alterImg(){
    this.showImg = !this.showImg;
  }

  public getEventos(): void {
    this.http.get(`https://localhost:5001/api/eventos`).subscribe(
      response => {
        this.eventos = response
        this.filtredEvents = this.eventos
      },
      error => console.log(error)
    )
  }

}
