import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Event } from '@app/models/Event';
import { Part } from '@app/models/Part';
import { EventService } from '@app/services/event.service';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss']
})
export class EventDetailsComponent implements OnInit {

  event = {} as Event;
  form: FormGroup;
  state = 'post';

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

  constructor(private fb: FormBuilder,
              private localeService: BsLocaleService,
              private activatedRouter: ActivatedRoute,
              private router: Router,
              private eventService: EventService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService)
    {
      this.localeService.use('pt-br');
    }

  loadEvent(): void {
    const eventIdParam = this.activatedRouter.snapshot.paramMap.get('id');

    if (eventIdParam !== null) {
      this.spinner.show();
      this.state = 'put';
      this.eventService.getEventById(+eventIdParam).subscribe(
        (event: Event) => {
          this.event = {...event};
          this.form.patchValue(this.event);
        },
        (error: any) => {
          this.toastr.error('Erro ao tentar carregar o evento', 'Erro!');
          console.error(error);
        }
      ).add(() => this.spinner.hide());
    }
  }

  ngOnInit(): void {
    this.loadEvent();
    this.validation();
  }

  public validation(): void {
    this.form = this.fb.group ({
      theme: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dateEvent: ['', Validators.required],
      capacity: ['', [Validators.required, Validators.max(12000), Validators.min(20)]],
      callNumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email ]],
      imageURL: ['', Validators.required],
      parts: this.fb.array([])
    });
  }

  addPart(): void {
    this.parts.push(this.createPart({id: 0}as Part));
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
          this.loadEvent();
          this.router.navigate([`events/details/${eventReturn.id}`]);
          this.toastr.success(`Evento ${this.event.id ? 'atualizado' : 'criado' } com sucesso`, 'Ok!');
      },
        (error: any) => {
          this.toastr.error(`Erro ao tentar ${this.event.id ? 'atualizar' : 'criar' } o evento`, 'Erro!');
          console.error(error);
        }

      ).add(() => this.spinner.hide());
    }
  }
}
