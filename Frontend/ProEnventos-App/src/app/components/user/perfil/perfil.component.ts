import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {
  public form!: FormGroup;

  get f(): any {
    return this.form.controls;
  }
  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.validation();
  }

  private validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('senha', 'confirmarSenha')
    };

    this.form = this.fb.group({
    titulo: ['', Validators.required],
    primeiroNome: ['',[Validators.required]],
    ultimoNome: ['',[Validators.required]],
    email: ['',[Validators.required, Validators.email]],
    funcao: ['', Validators.required],
    descricao: ['', Validators.required],
    telefone: ['',[Validators.required]],
    senha: ['',[Validators.required, Validators.minLength(6)]],
    confirmarSenha: ['',[Validators.required]],
    }, formOptions);
  }
}
