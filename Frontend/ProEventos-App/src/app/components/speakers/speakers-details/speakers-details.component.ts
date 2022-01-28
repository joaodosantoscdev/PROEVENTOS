import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Speaker } from '@app/models/Speaker';
import { SpeakerService } from '@app/services/speaker.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-speakers-details',
  templateUrl: './speakers-details.component.html',
  styleUrls: ['./speakers-details.component.scss']
})
export class SpeakersDetailsComponent implements OnInit {

  speaker = {} as Speaker;
  public form!: FormGroup;
  public formOutputResult = '';
  public colorDescription = '';

  constructor(private fb: FormBuilder,
              public speakerService: SpeakerService,
              private toastr: ToastrService,
              private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
    this.validation();
    this.loadSpeaker();
  }

  private validation(): void {
    this.form = this.fb.group({
      cv: [''],
    });
  }

  private loadSpeaker(): void {
    this.spinner.show();

    this.speakerService.getSpeakerByToken()
    .subscribe(
      (speakerResult: Speaker) => {
        this.speaker.cv = speakerResult.cv;
        this.form.patchValue(this.speaker);
    },
    (error: any) => {
      this.toastr.error('Erro ao carregar o palestrante', 'Erro!');
      console.error(error);
    }
    ).add(() => this.spinner.hide());
  }

  private verifyForm(): void {
        this.speakerService.put({ ...this.form.value })
        .subscribe(
          (speakerResult: Speaker) => {
            this.toastr.success('Mini currículo atualizado', 'Sucesso');
            this.speaker.cv = speakerResult.cv;
            this.loadSpeaker();
            debugger;
          },
          (error: any) => {
            this.toastr.error('Erro ao atualizar sua descrição como palestrante', 'Erro!');
            console.error(error);
          }
        ).add(() => this.spinner.hide());
  }



      public get f(): any {
        return this.form.controls;
      }

      public resetForm(event: any): void {
        event.preventDefault();
        this.form.reset();
      }

      onSubmit(): void {
        this.verifyForm();
      }
    }
