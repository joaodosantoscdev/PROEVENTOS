import { Component, OnInit } from '@angular/core';

import { UserUpdate } from '@app/models/identity/userUpdate';
import { UserService } from '@app/services/user.service';
import { environment } from '@environments/environment';

import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  public user = {} as UserUpdate;
  public file: File;
  public imageURL = '';

  public get isSpeaker(): boolean {
    return this.user.function === 'Palestrante';
  }

  constructor(private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private userService: UserService) { }

  ngOnInit(): void {
  }

  public setFormValue(user: UserUpdate): void {
    this.user = user;
    if (this.user.imageURL) {
      this.imageURL = environment.apiURL + `Resources/Profile/${this.user.imageURL}`;
    } else {
      this.imageURL = 'assets/img/profile-default.png';
    }

  }

  onFileChange(ev: any): void {
    const reader = new FileReader();

    reader.onload = (event: any) => this.imageURL = event.target.result;
    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImage();
  }

  private uploadImage(): void {
    this.spinner.show();
    this.userService
    .postUpload(this.file).subscribe(
      () => {
        this.toastr.success('Imagem atualizada com sucesso', 'Sucesso!');
      },
      (error: any) => {
        this.toastr.error('Error ao atualizar a imagem de perfil', 'Erro!');
        console.error(error);
      }
      ).add(() => this.spinner.hide());
  }

}
