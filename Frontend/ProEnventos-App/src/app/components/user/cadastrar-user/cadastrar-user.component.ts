import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cadastrar-user',
  templateUrl: './cadastrar-user.component.html',
  styleUrls: ['./cadastrar-user.component.scss']
})
export class CadastrarUserComponent implements OnInit {
  user = {} as User;
  public form!: FormGroup;

  constructor(private fb: FormBuilder, private accountService: AccountService,
              private router: Router, private toastr: ToastrService) { }
  ngOnInit() {
    this.validation();
  }
  public get f(): any {
    return this.form.controls;
  }
  private validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmarPassword')
    };
    this.form = this.fb.group({
      primeiroNome: ['',[Validators.required]],
      ultimoNome: ['',[Validators.required]],
      email: ['',[Validators.required, Validators.email]],
      userName: ['',[Validators.required]],
      password: ['',[Validators.required, Validators.minLength(4)]],
      confirmarPassword: ['',[Validators.required]],
    },formOptions);
  }
  public register(): void{
    this.user = { ...this.form.value };
    this.accountService.register(this.user).subscribe(
      () => this.router.navigateByUrl('/dashboard'),
      (error: any) => this.toastr.error(error.error)
    );
  }
}
