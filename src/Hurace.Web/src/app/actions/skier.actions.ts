import { SkierDto } from '../dtos/skier.dto';

export class GetAllSkiers {
    static readonly type = '[Skier] GetAll';
}

export class GetSkierById {
    static readonly type = '[Skier] GetById';
    constructor(public id: number) { }
}

export class SaveSkier {
    static readonly type = '[Skier] Save';
    constructor(public skier: SkierDto) { }
}

export class RemoveSkier {
    static readonly type = '[Skier] Remove';
    constructor(public id: number) { }
}

export class NewSkier {
    static readonly type = '[Skier] New';
}
