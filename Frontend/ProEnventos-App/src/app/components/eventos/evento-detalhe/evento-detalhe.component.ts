import { DatePipe } from '@angular/common';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/models/Evento';
import { Lote } from 'src/models/Lote';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
  providers: [DatePipe],
})
export class EventoDetalheComponent implements OnInit {
  loteAtual = {id: 0, nome: '', indice: 0}
  modalRef: BsModalRef;
  eventoId!: number;
  evento = {} as Evento;
  public form!: FormGroup;
  estadoSalvar = 'post';

  get editMode(): boolean {
    return this.estadoSalvar == 'put';
  }
  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }
  get f(): any {
    return this.form.controls;
  }

  get bsconfig(): any {
    return{
      adaptivePosition: true, dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',  showWeekNumbers: false,
    };
  }

  // get bsconfigLote(): any {
  //   return{
  //     adaptivePosition: true, dateInputFormat: 'DD/MM/YYYY',
  //     containerClass: 'theme-default',  showWeekNumbers: false,
  //   };
  // }
  constructor(private fb: FormBuilder, private localeService: BsLocaleService,
              private activetedRouter: ActivatedRoute, private eventoService: EventoService,
              private spinner: NgxSpinnerService, private toastr: ToastrService,
              private datePipe: DatePipe, private router: Router,
              private loteService: LoteService, private modalService: BsModalService) {
    this.localeService.use('pt-br')
   }

  ngOnInit() {
    this.carregarEvento();
    this.validation();
  }

  public carregarEvento(): void{
    this.eventoId = +this.activetedRouter.snapshot.paramMap.get('id');

    if(this.eventoId !== null && this.eventoId !== 0){
      this.spinner.show();
      this.estadoSalvar = 'put';

      this.eventoService.getEventoById(this.eventoId).subscribe(
        (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
          //this.carregarLotes();
          this.evento.lotes.forEach(lote => {
            this.lotes.push(this.createLote(lote));
          })
        },
        (error: any) => {
          this.toastr.error('Erro ao carregar evento.', 'Erro!')
          console.log(error);
        },

      ).add(() => this.spinner.hide());
    }
  }

  // public carregarLotes(): void {
  //   this.loteService.getLotesByEventoId(this.eventoId).subscribe(
  //     (lotesRetorno: Lote[]) => {
  //       lotesRetorno.forEach(lote => {
  //         this.lotes.push(this.createLote(lote));
  //       })
  //     },
  //     (error: any) => {
  //       this.toastr.error('Erro ao tentar carregar lotes', 'Erro!');
  //       console.log(error);
  //     },

  //   ).add(() => this.spinner.hide());
  // }

  public validation(): void {
    this.form = this.fb.group({
    tema: ['',[Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
    local: ['',[Validators.required,Validators.maxLength(100)]],
    dataEvento: ['',Validators.required],
    qtdPessoas: ['',[Validators.required, Validators.max(12000)]],
    telefone: ['',Validators.required],
    email: ['',[Validators.required, Validators.email]],
    imagemURL: ['',Validators.required],
    lotes: this.fb.array([])
    });
  }
  addLote(): void {
    this.lotes.push(this.createLote({id: 0} as Lote))
  }

  createLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio, Validators.required],
      dataFim: [lote.dataFim, Validators.required]
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl | null): any {
    return {'is-invalid': campoForm?.errors && campoForm?.touched};
  }

  public mudarValorData(value: Date, indice: number, campo:string): void {
    this.lotes.value[indice][campo] = value;
  }

  public retornaTituloLote(nome: string): string {
    return nome === null || nome === '' ? 'Nome do Lote' : nome
  }
  public salvarEvento(): void{
    this.spinner.show();
    if(this.form.valid){

      this.evento = (this.estadoSalvar === 'post') ?
      {...this.form.value} : {id: this.evento.id, ...this.form.value};

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        (eventoRetorno: Evento) => {
          this.toastr.success('Evento salvo com sucesso!', 'Sucesso');
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`]);
        },
        (error: any) => {
          console.log(error);
          this.spinner.hide();
          this.toastr.error('Erro ao salvar evento', 'Erro');
        },
        () => this.spinner.hide()
      );
    }
  }

  public salvarLotes(): void {
    if(this.form.controls.lotes.valid){
      this.spinner.show();
      this.loteService.saveLote(this.eventoId, this.form.value.lotes).subscribe(
        () => {
          this.toastr.success('Lotes salvos com sucesso!', 'Sucesso!');
          this.lotes.reset();
        },
        (error: any) => {
          this.toastr.error('Erro ao tentar salvar Lotes', 'Erro!')
          console.log(error);
        },
      ).add(() => this.spinner.hide());
    }
  }

  public removerLote(template: TemplateRef<any>,indice: number): void {
    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;
    this.modalRef = this.modalService.show(template, {class:'modal-sm'})
  }

  confirmDeleteLote(): void {
    this.modalRef.hide();
    this.spinner.show();
    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      () => {
        this.toastr.success('Lote excluído com sucesso', 'Sucesso');
        this.lotes.removeAt(this.loteAtual.indice);
      },
      (error: any) => {
        this.toastr.error(`Erro ao tentar excluir o lote ${this.loteAtual.id}`, 'Erro!');
        console.log(error);
      },
    ).add(() => this.spinner.hide());
  }

  declineDeleteLote(): void {
    this.modalRef.hide();
  }
}
