import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { EventoService } from '@app/services/evento.service';
import { Evento } from '@app/models/Evento';
import { environment } from '@environments/environment';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public eventoId = 0;
  public widthImg = 150;
  public marginImg = 2;
  public showImg = true;
  public pagination = {} as Pagination;
  termoBuscaChanged: Subject<string> = new Subject<string>();


  public filtrarEventos(evt: any): void {
    if (this.termoBuscaChanged.observers.length === 0) {
      this.termoBuscaChanged
        .pipe(debounceTime(1000))
        .subscribe((filtrarPor) => {
          this.spinner.show();
          this.eventoService
            .getEventos(
              this.pagination.currentPage,
              this.pagination.itemsPerPage,
              filtrarPor
            )
            .subscribe(
              (paginatedResult: PaginatedResult<Evento[]>) => {
                this.eventos = paginatedResult.result;
                this.pagination = paginatedResult.pagination;
              },
              (error: any) => {
                this.spinner.hide();
                this.toastr.error('Erro ao Carregar os Eventos', 'Erro!');
              }
            )
            .add(() => this.spinner.hide());
        });
    }
    this.termoBuscaChanged.next(evt.value);
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  public ngOnInit(): void {
    this.pagination = {currentPage: 1, itemsPerPage: 3, totalItems: 1} as Pagination;
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
    this.spinner.show();
    this.eventoService.getEventos(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(
      (paginatedResult: PaginatedResult<Evento[]>) => {
        this.eventos = paginatedResult.result;
        this.pagination = paginatedResult.pagination;
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
  pageChanged(event): void {
    this.pagination.currentPage = event.page;
    this.carregarEventos();
  }

}
