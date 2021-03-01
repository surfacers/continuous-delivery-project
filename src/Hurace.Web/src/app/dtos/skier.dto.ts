import { Gender } from '../enums';

export interface SkierDto {
    id: number;
    firstName: string;
    lastName: string;
    gender: Gender;
    countryCode: string;
    birthDate: Date | null;
    image: string;
    isActive: boolean;
    isRemoved: boolean;
}
