import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';

@Component({
  selector: 'app-cadastrar-user',
  templateUrl: './cadastrar-user.component.html',
  styleUrls: ['./cadastrar-user.component.scss']
})
export class CadastrarUserComponent implements OnInit {

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
    primeiroNome: ['',[Validators.required]],
    ultimoNome: ['',[Validators.required]],
    email: ['',[Validators.required, Validators.email]],
    userName: ['',[Validators.required]],
    senha: ['',[Validators.required, Validators.minLength(6)]],
    confirmarSenha: ['',[Validators.required]],
    }, formOptions);
  }
  // public resetForm(): void {
  //   this.form.reset();
  // }

}
