import React, {useEffect, useState} from 'react';
import { Admin, Loading, Resource, ResourceElement } from 'react-admin';
import odataProvider, { OdataDataProvider } from "ra-data-odata-server";
import { getAuthToken } from "./authProvider";
import UserIcon from "@mui/icons-material/People";
import StarIcon from "@mui/icons-material/Star";
import SchoolIcon from "@mui/icons-material/School";
import WorkIcon from "@mui/icons-material/Work";
import MicIcon from "@mui/icons-material/Mic";
import ArticleIcon from "@mui/icons-material/Article";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";

import { UsersList, UserCreate, UserEdit, UserShow } from "./services/users";
import {
  ExperienceList,
  ExperienceShow,
  ExperienceEdit,
  ExperienceCreate
} from "./services/experience";
import {
  EducationList,
  EducationShow,
  EducationCreate,
  EducationEdit
} from "./services/education";
import { PortofolioList, PortofolioShow, PortofolioUpdate, PortofolioCreate } from "./services/portofolio";
import {
  BlogList,
  BlogShow,
  BlogCreate,
  BlogEdit
} from "./services/blog";
import {
  EngagementList,
  EngagementShow,
  EngagementCreate,
  EngagementEdit
} from "./services/engagement";
import {
  PublicationList,
  PublicationShow,
  PublicationCreate,
  PublicationEdit
} from "./services/publication";
import {
  ProfileList,
  ProfileShow,
  ProfileCreate,
  ProfileEdit
} from "./services/profile";
import authProvider from "./authProvider";
import "./App.css";

const resourceList = (permissions: any) => [
  permissions === "admin" ? (
    <Resource
      name="users"
      list={UsersList}
      create={UserCreate}
      edit={UserEdit}
      icon={UserIcon}
      show={UserShow}
    />
  ) : null,
  (
    <Resource
      name="experiences"
      icon={WorkIcon}
      list={ExperienceList}
      show={ExperienceShow}
      edit={ExperienceEdit}
      create={ExperienceCreate}
    />
  ),
  permissions === "admin" ? (
    <Resource
      name="education"
      icon={SchoolIcon}
      list={EducationList}
      show={EducationShow}
      edit={EducationEdit}
      create={EducationCreate}
    />
  ) : null,
  permissions === "admin" ? (
    <Resource
      name="portofolio"
      list={PortofolioList}
      show={PortofolioShow}
      edit={PortofolioUpdate}
      create={PortofolioCreate}
    />
  ) : null,
  permissions === "admin" ? (
    <Resource
      name="blogs"
      list={BlogList}
      show={BlogShow}
      edit={BlogEdit}
      create={BlogCreate}
    />
  ) : null,
  permissions === "admin" ? (
    <Resource
      name="engagements"
      icon={MicIcon}
      list={EngagementList}
      show={EngagementShow}
      edit={EngagementEdit}
      create={EngagementCreate}
    />
  ) : null,
  permissions === "admin" ? (
    <Resource
      name="publications"
      icon={ArticleIcon}
      list={PublicationList}
      show={PublicationShow}
      edit={PublicationEdit}
      create={PublicationCreate}
    />
  ) : null,
  permissions === "admin" ? (
    <Resource
      name="profiles"
      icon={AccountCircleIcon}
      list={ProfileList}
      show={ProfileShow}
      edit={ProfileEdit}
      create={ProfileCreate}
    />
  ) : null
].filter((element) => Boolean(element)).map(element => element as React.JSX.Element);

function App() {
  const [dataProvider, setDataProvider] = useState<OdataDataProvider>();
  useEffect(() => {
    odataProvider(
        `${import.meta.env.VITE_SERVICE_URL ?? ''}/api`,
        async () => ({
          fetchProxy: async (url, init) => {
            const token = getAuthToken();
            const headers = new Headers(init?.headers as HeadersInit | undefined);
            if (token) {
              headers.set('Authorization', `Bearer ${token}`);
            }
            const response = await fetch(url, { ...init, headers });
            const content = response.headers.get('content-type')?.includes('json')
              ? await response.json()
              : await response.text();
            return { content, response };
          },
        })
    ).then((p) => setDataProvider(p));
    return () => {
    };
  }, []);

  return (
      dataProvider ?
          <Admin title="Berv Admin" dataProvider={dataProvider} authProvider={authProvider}>
            {resourceList}
          </Admin> : (
              <Loading></Loading>
          )
  );
}

export default App;
