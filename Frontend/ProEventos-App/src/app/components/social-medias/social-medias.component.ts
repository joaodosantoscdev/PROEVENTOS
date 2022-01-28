import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { SocialMedia } from '@app/models/SocialMedia';
import { SocialMediaService } from '@app/services/socialMedia.service';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-social-medias',
  templateUrl: './social-medias.component.html',
  styleUrls: ['./social-medias.component.scss']
})
export class SocialMediasComponent implements OnInit {

  modalRef: BsModalRef;
  @Input() eventId = 0;
  public formSM: FormGroup;
  public currentSocialMedia = { id: 0, name: '', index: 0};

  get socialMedias(): FormArray {
    return this.formSM.get('socialMedias') as FormArray;
  }

  constructor( private socialMediaService: SocialMediaService,
               private fb: FormBuilder,
               private router: Router,
               private toastr: ToastrService,
               private spinner: NgxSpinnerService,
               private modalService: BsModalService) {}

  ngOnInit(): void {
    if (this.eventId === 0) {
      this.loadSocialMedia('speaker');
    }
    this.validation();
  }

  private loadSocialMedia(origin: string, id: number = 0): void {
    this.spinner.show();
    this.socialMediaService.getSocialMedia(origin, id).subscribe(
      (socialMediaResult: SocialMedia[]) => {
        socialMediaResult.forEach((socialMedia) => {
          this.socialMedias.push(this.createSocialMedia(socialMedia));
        });
      },
      (error: any) => {
        this.toastr.error('Não foi possível carregar as mídias sociais', 'Erro!');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

  public validation(): void {
    this.formSM = this.fb.group({
      socialMedias: this.fb.array([]),
    });
  }

  addSocialMedia(): void {
    this.socialMedias.push(this.createSocialMedia({id: 0} as SocialMedia));
  }

  createSocialMedia(socialMedia: SocialMedia): FormGroup {
    return this.fb.group({
      id: [socialMedia.id],
      name: [socialMedia.name, Validators.required],
      url: [socialMedia.url]
    });
  }

  public returnTitle(name: string): string {
    return name === null || name === '' ? 'Nome da Rede-Social' : name;
  }

  public cssValidator(formRule: FormControl | AbstractControl): any {
    return { 'is-invalid': formRule.errors && formRule.touched };
  }

  public saveSocialMedia(): void {
    let origin = 'speakerId';

    if (this.eventId !== 0) { origin = 'event'; }
    if (this.formSM.controls.socialMedias.valid) {
      this.spinner.show();
      this.socialMediaService.saveSocialMedia(origin, this.eventId, this.formSM.value.socialMedias).subscribe(
        (socialMediaReturn: any) => {
          this.toastr.success('Redes Sociais salvas com sucesso!', 'Ok!');
          debugger;
        },
        (error: any) => {
          this.toastr.error('Erro ao tentar salvar as redes sociais', 'Erro!');
          console.error(error);
        }
      ).add(() => this.spinner.hide());
    }
  }

  public removeSocialMedias(template: TemplateRef<any>, index: number): void  {

    this.currentSocialMedia.id = this.socialMedias.get(index + '.id').value;
    this.currentSocialMedia.name = this.socialMedias.get(index + '.name').value;
    this.currentSocialMedia.index = index;

    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirmDeleteSocialMedia(): void {
    let origin = 'speaker';
    this.modalRef.hide();
    this.spinner.show();

    if (this.eventId !== 0) { origin = 'event'; }

    this.socialMediaService.deleteSocialMedia(origin, this.eventId, this.currentSocialMedia.id).subscribe(
      () => {
        this.toastr.success('Rede social deletada com sucesso', 'Sucesso!');
        this.socialMedias.removeAt(this.currentSocialMedia.index);
      },
      (error: any) => {
        this.toastr.error(`Ocorreu um erro ao tentar deletar o Lote: ${this.currentSocialMedia.id}`, 'Erro!');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }
   declineDeleteSocialMedia(): void {
     this.modalRef.hide();
  }

}
