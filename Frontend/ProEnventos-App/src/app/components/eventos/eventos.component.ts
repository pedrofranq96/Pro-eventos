import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../../services/evento.service';
import { Evento } from 'src/models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {
  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public filtredEvents: Evento[] = [];
  public widthImg = 150;
  public marginImg = 2;
  public showImg = true;
  private filtroListado = '';

  public get listFilter(): string {
    return this.filtroListado;
  }

  public set listFilter(value: string){
    this.filtroListado = value;
    this.filtredEvents = this.listFilter ? this.eventFilter(this.listFilter) : this.eventos;
  }

  public eventFilter(filter: string): Evento[] {
    filter = filter.toLowerCase();
    return this.eventos.filter(
      (evento: {tema: string; local: string;}) => evento.tema.toLocaleLowerCase().indexOf(filter) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filter) !== -1

    );
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) { }

  public ngOnInit(): void {
    this.spinner.show();
    this.getEventos();
  }

  public alterImg(): void{
    this.showImg = !this.showImg;
  }

  public getEventos(): void {
    this.eventoService.getEventos().subscribe({
      next: (eventos: Evento[]) => {
        this.eventos = eventos
        this.filtredEvents = this.eventos
      },
      error:(error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar eventos.', 'Algo de inesperado aconteceu!');
      },
      complete: () => this.spinner.hide()
    });
  }
  openModal(template: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.toastr.success('O evento foi excluído com sucesso.', 'Excluído!');
  }

  decline(): void {
    this.modalRef?.hide();
  }
}
