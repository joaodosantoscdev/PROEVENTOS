import { DatePipe } from '@angular/common';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, NavigationEnd, Router, RouterEvent } from '@angular/router';

import { Event } from '@app/models/Event';
import { Part } from '@app/models/Part';
import { EventService } from '@app/services/event.service';
import { PartService } from '@app/services/part.service';
import { environment } from '@environments/environment';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss']
})
export class EventDetailsComponent implements OnInit {

  modalRef: BsModalRef;
  eventId: number;
  event = {} as Event;
  form: FormGroup;
  state = 'post';
  actualPart = {id: 0, name: '', index: 0};
  imageURL = 'assets/img/upload-img.png';
  file: File;

  get editMode(): boolean {
    return this.state === 'put';
  }

  get parts(): FormArray {
    return this.form.get('parts') as FormArray;
  }

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm A',
      isAnimated: true,
      containerClass: 'theme-dark-blue',
      showWeekNumbers: false
    };
  }

  get bsConfigParts(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      isAnimated: true,
      containerClass: 'theme-dark-blue',
      showWeekNumbers: false
    };
  }

  constructor(private fb: FormBuilder,
              private localeService: BsLocaleService,
              private activatedRouter: ActivatedRoute,
              private router: Router,
              private partService: PartService,
              private modalService: BsModalService,
              private eventService: EventService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService)
    {
      this.localeService.use('pt-br');
    }

  loadEvent(): void {
    this.eventId = +this.activatedRouter.snapshot.paramMap.get('id');
    if (this.eventId !== null && this.eventId !== 0) {
      this.spinner.show();
      this.state = 'put';

      this.eventService.getEventById(this.eventId).subscribe(
        (eventResult: Event) => {
          this.event = {...eventResult};
          this.form.patchValue(this.event);
          if (this.event.imageURL !== '') {
            this.imageURL = `${environment.apiURL}Resources/Images/${this.event.imageURL}`;
          }
          this.loadParts();
        },
        (error: any) => {
          this.toastr.error('Erro ao tentar carregar o evento', 'Erro!');
          console.error(error);
        }
      ).add(() => this.spinner.hide());
    }
  }

  loadParts(): void {
    this.partService.getPartsByEventId(this.eventId).subscribe(
      (partsReturn: Part[]) => {
      partsReturn.forEach(part => {
        this.parts.push(this.createPart(part));
      });
      },
      (error: any) => {
        this.toastr.error('Erro ao tentar carregar os lotes', 'Erro!');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

  ngOnInit(): void {
    this.validation();
    this.loadEvent();
  }

  public validation(): void {
    this.form = this.fb.group ({
      theme: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dateEvent: ['', Validators.required],
      capacity: ['', [Validators.required, Validators.max(12000), Validators.min(20)]],
      callNumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email ]],
      imageURL: [''],
      parts: this.fb.array([])
    });
  }

  addPart(): void {
    this.parts.push(this.createPart({id: 0} as Part));
  }

  createPart(part: Part): FormGroup {
    return this.fb.group({
      id: [part.id],
      name: [part.name, Validators.required],
      price: [part.price, Validators.required],
      quantity: [part.quantity, Validators.required],
      dateInitial: [part.dateInitial],
      dateEnd: [part.dateEnd],
    });
  }

  public returnTitlePart(name: string): string {
    return name === null || name === '' ? 'Nome do lote' : name;
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(formRule: FormControl | AbstractControl): any {
    return { 'is-invalid': formRule.errors && formRule.touched };
  }

  public saveEvent(): void {
    this.spinner.show();
    if (this.form.valid) {
      this.event = ( this.state === 'post')
        ? {...this.form.value}
        : { id: this.event.id, ...this.form.value };
      this.eventService[this.state](this.event).subscribe(
        (eventReturn: Event) => {
          this.toastr.success(`Evento ${this.event.id ? 'atualizado' : 'criado' } com sucesso`, 'Ok!');
          this.event = {...eventReturn};
          this.form.patchValue(this.event);
          this.router.navigate([`events/details/${eventReturn.id}`]);
      },
        (error: any) => {
          this.toastr.error(`Erro ao tentar ${this.event.id ? 'atualizar' : 'criar' } o evento`, 'Erro!');
          console.error(error);
        }

      ).add(() => this.spinner.hide());
    }
  }

  public saveParts(): void {
    if (this.form.controls.parts.valid) {
      this.spinner.show();
      this.partService.SavePart(this.eventId, this.form.value.parts).subscribe(
        (partReturn: any) => {
          this.toastr.success('Lotes salvos com Sucesso!', 'Ok!');
          this.router.navigate([`events/details/${partReturn.id}`]);
        },
        (error: any) => {
          this.toastr.error('Erro ao tentar salvar os Lotes', 'Erro!');
          console.error(error);
        }
      ).add(() => this.spinner.hide());
    }
  }

  public removeParts(template: TemplateRef<any>, index: number): void  {

    this.actualPart.id = this.parts.get(index + '.id').value;
    this.actualPart.name = this.parts.get(index + '.name').value;
    this.actualPart.index = index;

    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirmDeletePart(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.partService.deletePart(this.eventId, this.actualPart.id).subscribe(
      () => {
        this.toastr.success('Lote deletado com sucesso', 'Sucesso!');
        this.parts.removeAt(this.actualPart.index);
      },
      (error: any) => {
        this.toastr.error(`Ocorreu um erro ao tentar deletar o Lote: ${this.actualPart.id}`, 'Erro!');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

  declineDeletePart(): void {
    this.modalRef.hide();
  }

  onFileChange(ev: any): void {
    const reader = new FileReader();

    reader.onload = (event: any) => this.imageURL = event.target.result;
    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImage();
  }

  uploadImage(): void {
    this.spinner.show();
    this.eventService.postUpload(this.eventId, this.file).subscribe(
      () => {
        this.loadEvent();
        this.toastr.success('Imagem atualizada com sucesso', 'Sucesso!');
      },
      (error: any) => {
        this.toastr.error('Erro ao atualizar a imagem', 'Erro!');
        console.log(error);
    }
      ).add(() => this.spinner.hide());
  }
}
