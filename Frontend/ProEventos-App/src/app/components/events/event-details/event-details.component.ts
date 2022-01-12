import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { Event } from '@app/models/Event';
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
              private router: ActivatedRoute,
              private eventService: EventService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService)
    {
      this.localeService.use('pt-br');
    }

  loadEvent(): void {
    const eventIdParam = this.router.snapshot.paramMap.get('id');

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
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(formRule: FormControl): any {
    return { 'is-invalid': formRule.errors && formRule.touched };
  }

  public saveEvent(): void {
    this.spinner.show();
    if (this.form.valid) {
      this.event = ( this.state === 'post')
        ? {...this.form.value}
        : { id: this.event.id, ...this.form.value };
      this.eventService[this.state](this.event).subscribe(
        () => {
          this.loadEvent();
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
