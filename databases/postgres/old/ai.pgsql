--
-- PostgreSQL database dump
--

-- Dumped from database version 14.4
-- Dumped by pg_dump version 14.4

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: edge; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.edge (
    start integer NOT NULL,
    ending integer NOT NULL
);


ALTER TABLE public.edge OWNER TO postgres;

--
-- Name: figure; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.figure (
    name text NOT NULL
);


ALTER TABLE public.figure OWNER TO postgres;

--
-- Name: figure_appearance; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.figure_appearance (
    figure text NOT NULL,
    start bigint NOT NULL,
    ending bigint NOT NULL,
    CONSTRAINT "start < ending" CHECK ((start < ending))
);


ALTER TABLE public.figure_appearance OWNER TO postgres;

--
-- Name: subfigure; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.subfigure (
    id integer NOT NULL,
    "parent figure" text NOT NULL,
    "referenced figure" text NOT NULL
);


ALTER TABLE public.subfigure OWNER TO postgres;

--
-- Data for Name: edge; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.edge (start, ending) FROM stdin;
\.


--
-- Data for Name: figure; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.figure (name) FROM stdin;
a
b
\.


--
-- Data for Name: figure_appearance; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.figure_appearance (figure, start, ending) FROM stdin;
\.


--
-- Data for Name: subfigure; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.subfigure (id, "parent figure", "referenced figure") FROM stdin;
\.


--
-- Name: edge edge_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edge
    ADD CONSTRAINT edge_pkey PRIMARY KEY (start, ending);


--
-- Name: figure_appearance figure_appearance_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.figure_appearance
    ADD CONSTRAINT figure_appearance_pkey PRIMARY KEY (figure, start, ending);


--
-- Name: figure figure_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.figure
    ADD CONSTRAINT figure_pkey PRIMARY KEY (name);


--
-- Name: subfigure id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subfigure
    ADD CONSTRAINT id UNIQUE (id);


--
-- Name: subfigure subfigure_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subfigure
    ADD CONSTRAINT subfigure_pkey PRIMARY KEY ("parent figure");


--
-- Name: edge end is a subfigure; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edge
    ADD CONSTRAINT "end is a subfigure" FOREIGN KEY (ending) REFERENCES public.subfigure(id);


--
-- Name: figure_appearance figure; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.figure_appearance
    ADD CONSTRAINT figure FOREIGN KEY (figure) REFERENCES public.figure(name);


--
-- Name: subfigure parent figure is a figure; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subfigure
    ADD CONSTRAINT "parent figure is a figure" FOREIGN KEY ("parent figure") REFERENCES public.figure(name) NOT VALID;


--
-- Name: subfigure referenced figure is a figure; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subfigure
    ADD CONSTRAINT "referenced figure is a figure" FOREIGN KEY ("referenced figure") REFERENCES public.figure(name) NOT VALID;


--
-- Name: edge start is a subfigure; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edge
    ADD CONSTRAINT "start is a subfigure" FOREIGN KEY (start) REFERENCES public.subfigure(id);


--
-- PostgreSQL database dump complete
--

