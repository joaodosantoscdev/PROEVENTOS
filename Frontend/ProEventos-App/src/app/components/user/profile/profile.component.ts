import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/userUpdate';
import { UserService } from '@app/services/user.service';

import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  form!: FormGroup;
  userUpdate = {} as UserUpdate;

  get f(): any {
    return this.form.controls;
  }

  constructor( private fb: FormBuilder,
               public userService: UserService,
               private toastr: ToastrService,
               private router: Router,
               private spinner: NgxSpinnerService ) { }

  ngOnInit(): void {
    this.validation();
    this.loadUser();
  }

  onSubmit(): void {
    this.updateUser();
  }


  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }

  public validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmPassword')
    };

    this.form = this.fb.group ({
      userName: [''],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      title: ['NaoInformado', Validators.required],
      function: ['NaoInformado', Validators.required],
      description: ['', [Validators.required, Validators.minLength(20), Validators.maxLength(300) ]],
      password: ['', [Validators.nullValidator, Validators.minLength(6)]],
      confirmPassword: ['', Validators.nullValidator],
    }, formOptions);
  }

  updateUser(): void {
    this.userUpdate = { ...this.form.value };
    this.spinner.show();
    this.userService.updateUser(this.userUpdate).subscribe(
      () => {
        this.toastr.success('Usuário carregado', 'Sucesso!');
      },
      (error: any) => {
        this.toastr.error('Erro ao tentar atualizar as informações do usuário.', 'Erro!');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

  private loadUser(): void {
    this.spinner.show();
    this.userService.getUser().subscribe(
      (userReturn: UserUpdate) => {
        this.userUpdate = userReturn;
        this.form.patchValue(this.userUpdate);
        this.toastr.success('Usuário carregado', 'Sucesso!');
      },
      (error: any) => {
        this.toastr.error('Erro ao carregar o usuário', 'Erro!');
        console.error(error);
        this.router.navigate(['/dashboard']);
      }
    ).add(() => this.spinner.hide());
  }

}
