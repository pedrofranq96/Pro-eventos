import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { EventoService } from '@app/services/evento.service';
import { Evento } from 'src/models/Evento';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public filtredEvents: Evento[] = [];
  public eventoId = 0;
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
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  public ngOnInit(): void {
    this.spinner.show();
    this.carregarEventos();
  }

  public alterImg(): void{
    this.showImg = !this.showImg;
  }

  public returnImage(imagemURL: string): string{
    return (imagemURL !== '') ? `${environment.apiURL}Resources/Images/${imagemURL}`
      : 'assets/semImagem.jpeg';
  }

  public carregarEventos(): void {
    this.eventoService.getEventos().subscribe(
      (eventos: Evento[]) => {
        this.eventos = eventos
        this.filtredEvents = this.eventos
      },
      (error: any) => {
        console.log(error);
        this.toastr.error('Erro ao carregar eventos.', 'Algo de inesperado aconteceu!');
      }
    ).add(()=> this.spinner.hide());
  }
  openModal(event: any ,template: TemplateRef<any>,eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
        if(result.message === 'Excluído'){
          this.toastr.success('O evento foi excluído com sucesso.', 'Excluído!');
          this.carregarEventos();
        }
      },
      (error: any) => {
        console.log(error);
        this.toastr.error(`Erro ao tentar excluir evento ${this.eventoId}`, 'Erro');
      },
    ).add(() => this.spinner.hide());
  }

  decline(): void {
    this.modalRef?.hide();
  }
  detalheEvento(id: number): void{
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

}
