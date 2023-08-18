import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/models/Evento';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
  providers: [DatePipe],
})
export class EventoDetalheComponent implements OnInit {
  evento = {} as Evento;
  public form!: FormGroup;
  estadoSalvar = 'post';

  get f(): any {
    return this.form.controls;
  }

  get bsconfig(): any {
    return{
      adaptivePosition: true, dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',  showWeekNumbers: false,
    };
  }
  constructor(private fb: FormBuilder, private localeService: BsLocaleService,
              private router: ActivatedRoute, private eventoService: EventoService,
              private spinner: NgxSpinnerService, private toastr: ToastrService,
              private datePipe: DatePipe) {
    this.localeService.use('pt-br')
   }

  ngOnInit() {
    this.carregarEvento();
    this.validation();
  }

  public carregarEvento(): void{
    const eventoIdParam = this.router.snapshot.paramMap.get('id');
    if(eventoIdParam !== null){
      this.spinner.show();
      this.estadoSalvar = 'put';

      this.eventoService.getEventoById(+eventoIdParam).subscribe(
        (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao carregar evento.', 'Erro!')
          console.log(error);
        },
        () => this.spinner.hide(),
      );
    }
  }
  public validation(): void {
    this.form = this.fb.group({
    tema: ['',[Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
    local: ['',[Validators.required,Validators.maxLength(100)]],
    dataEvento: ['',Validators.required],
    qtdPessoas: ['',[Validators.required, Validators.max(12000)]],
    telefone: ['',Validators.required],
    email: ['',[Validators.required, Validators.email]],
    imagemURL: ['',Validators.required],
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl): any {
    return {'is-invalid': campoForm?.errors && campoForm?.touched};
  }

  public salvarAlteracao(): void{
    this.spinner.show();
    if(this.form.valid){

      this.evento = (this.estadoSalvar === 'post') ?
      {...this.form.value} : {id: this.evento.id, ...this.form.value};

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        () => this.toastr.success('Evento salvo com sucesso', 'Sucesso'),
        (error: any) => {
          console.log(error);
          this.spinner.hide();
          this.toastr.error('Erro ao salvar evento', 'Erro');
        },
        () => this.spinner.hide()
      );
    }
  }
}
